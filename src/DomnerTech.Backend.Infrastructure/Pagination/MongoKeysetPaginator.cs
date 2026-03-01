using DomnerTech.Backend.Application;
using DomnerTech.Backend.Application.Caching;
using DomnerTech.Backend.Application.Extensions;
using DomnerTech.Backend.Application.Pagination;
using DomnerTech.Backend.Domain.Entities;
using DomnerTech.Backend.Infrastructure.Caching.Redis;
using DomnerTech.Backend.Infrastructure.MongoDb;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Reflection;
using System.Security;
using System.Text.Json;
using DomnerTech.Backend.Application.Json;

namespace DomnerTech.Backend.Infrastructure.Pagination;

public sealed class MongoKeysetPaginator<T>(
    IMongoDbContextFactory dbContextFactory,
    ICursorSerializer serializer,
    IRedisCache redisCache,
    AppSettings appSettings,
    SortProfile<T> sortProfile)
    : IKeysetPaginator<T>
    where T : IBaseEntity, ITenantEntity
{
    public async Task<KeysetPageResult<T>> PaginateAsync(
        string dbName,
        ObjectId tenantId,
        KeysetPageRequest request,
        FilterDefinition<T>? userFilter = null,
        CancellationToken ct = default)
    {
        var collection = dbContextFactory.Create(dbName).Database.GetCollection<T>();
        var builder = Builders<T>.Filter;

        var tenantFilter = builder.Eq(x => x.CompanyId, tenantId);
        // User filter
        var baseFilter = userFilter ?? builder.Empty;
        // Merge safely
        var filter = builder.And(tenantFilter, baseFilter);
        var fields = sortProfile.Resolve(request.SortKey);
        filter = ApplyCursorFilter(request, serializer, tenantId, filter, builder, fields);

        var sort = BuildSort(fields, request.Direction);

        var items = await collection
            .Find(filter)
            .Sort(sort)
            .Limit(request.PageSize + 1)
            .ToListAsync(ct);

        var hasNext = items.Count > request.PageSize;

        if (hasNext)
            items.RemoveAt(items.Count - 1);

        //if (request.Direction == CursorDirection.Backward)
        //    items.Reverse();

        var nextCursor = hasNext
            ? BuildCursor(tenantId, fields, items[^1])
            : null;

        var prevCursor = !string.IsNullOrEmpty(request.Cursor)
            ? BuildCursor(tenantId, fields, items[0])
            : null;

        long? total = null;

        if (request.IncludeTotalCount)
        {
            total = await redisCache.RedisFallbackAsync<long?>(
                $":paging:count:{tenantId}:{collection.CollectionNamespace.CollectionName}",
                new CacheEntryOptions
                {
                    AbsoluteExpiration = DateTime.UtcNow.AddSeconds(appSettings.MongoDatabases.Paging.CacheTtl)
                },
                async () =>
                {
                    var count = await collection.CountDocumentsAsync(tenantFilter, cancellationToken: ct);
                    return count > 0 ? count : null;
                });
        }

        return new KeysetPageResult<T>
        {
            Items = items,
            HasNextPage = hasNext,
            HasPreviousPage = !string.IsNullOrEmpty(request.Cursor),
            NextCursor = nextCursor,
            PreviousCursor = prevCursor,
            TotalItems = total
        };
    }

    private static FilterDefinition<T> ApplyCursorFilter(
        KeysetPageRequest request,
        ICursorSerializer serializer,
        ObjectId tenantId,
        FilterDefinition<T> filter,
        FilterDefinitionBuilder<T> builder,
        IReadOnlyList<CursorField<T>> fields)
    {
        if (string.IsNullOrWhiteSpace(request.Cursor))
            return filter;

        var decoded = serializer.Deserialize<CompositeCursor>(request.Cursor);

        if (decoded.TenantId != tenantId)
            throw new SecurityException("Tenant mismatch");

        // Build lexicographic (composite) comparison:
        // OR(
        //   f0 op v0,
        //   (f0 == v0 && f1 op v1),
        //   (f0 == v0 && f1 == v1 && f2 op v2),
        //   ...
        // )
        var orFilters = new List<FilterDefinition<T>>();
        for (var i = 0; i < fields.Count; i++)
        {
            var andParts = new List<FilterDefinition<T>>();

            // equality for all prefix fields
            for (var j = 0; j < i; j++)
            {
                var prefixField = fields[j];
                var prefixName = prefixField.GetFieldName();
                if (!decoded.Values.TryGetValue(prefixName, out var prefixValueRaw))
                {
                    // if value missing, treat as no-match for this branch
                    andParts.Add(builder.Where(_ => false));
                }
                else
                {
                    var prefixValue = TryConvertValue(prefixValueRaw, GetMemberType(typeof(T), prefixField));
                    andParts.Add(builder.Eq(prefixField.FieldFunc!, prefixValue));
                }
            }

            var current = fields[i];
            var currentName = current.GetFieldName();
            if (!decoded.Values.TryGetValue(currentName, out var currentValueRaw))
            {
                // missing value -> skip this branch
                continue;
            }

            var currentValue = TryConvertValue(currentValueRaw, GetMemberType(typeof(T), current));

            // Determine operator based on sort direction on this field and requested cursor direction
            // If field is descending: forward means Lt (smaller values come after), backward means Gt
            var opIsLess = request.Direction == CursorDirection.Forward ? current.Descending : !current.Descending;
            var comparison = opIsLess
                ? builder.Lt(current.FieldFunc!, currentValue)
                : builder.Gt(current.FieldFunc!, currentValue);

            andParts.Add(comparison);
            orFilters.Add(builder.And(andParts));
        }

        if (orFilters.Count == 0)
            return filter;

        filter = builder.And(filter, builder.Or(orFilters));
        return filter;
    }
    private string BuildCursor(ObjectId tenantId, IReadOnlyList<CursorField<T>> fields, T entity)
    {
        var dict = new Dictionary<string, object?>();

        foreach (var f in fields)
        {
            var name = f.GetFieldName();

            // Try to get value via expression (preferred) to handle nested props
            object? value;
            try
            {
                var expr = f.FieldFunc;
                var func = expr.Compile();
                value = func(entity);
            }
            catch
            {
                // fallback to reflection on top-level property name
                var prop = typeof(T).GetProperty(name, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
                value = prop?.GetValue(entity);
            }

            dict[name] = value!;
        }

        return serializer.Serialize(new CompositeCursor
        {
            TenantId = tenantId,
            Values = dict.ToDictionary(kv => kv.Key, kv => kv.Value!)
        });
    }
    private static Type GetMemberType(Type target, CursorField<T> field)
    {
        try
        {
            var name = field.GetFieldName();
            // try property first
            var prop = target.GetProperty(name, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
            if (prop != null) return Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType;

            // try field
            var fld = target.GetField(name, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
            if (fld != null) return Nullable.GetUnderlyingType(fld.FieldType) ?? fld.FieldType;
        }
        catch
        {
            // ignore and fallback to object
        }

        return typeof(object);
    }

    private static object? TryConvertValue(object? raw, Type? targetType)
    {
        if (raw == null) return null;
        targetType ??= typeof(object);

        // If it's already of the right type or assignable, use it.
        if (targetType.IsInstanceOfType(raw)) return raw;

        // Handle JsonElement (common when deserializing via System.Text.Json)
        if (raw is JsonElement je)
        {
            if (je.ValueKind == JsonValueKind.Null) return null;

            try
            {
                // Use JsonSerializer to map JsonElement to target CLR type
                return JsonConvert.DeserializeObject(je.GetRawText(), targetType);
            }
            catch
            {
                // fallthrough to next attempts
            }
        }

        // Strings representing values -> try deserialize
        if (raw is string s)
        {
            try
            {
                return JsonConvert.DeserializeObject(s, targetType);
            }
            catch
            {
                // try change type
            }
        }

        try
        {
            return Convert.ChangeType(raw, targetType);
        }
        catch
        {
            return raw;
        }
    }
    private static SortDefinition<T> BuildSort(
        IReadOnlyList<CursorField<T>> fields,
        CursorDirection direction)
    {
        var builder = Builders<T>.Sort;
        SortDefinition<T>? sort = null;

        // ReSharper disable once LoopCanBeConvertedToQuery
        foreach (var field in fields)
        {
            var desc = direction == CursorDirection.Forward
                ? field.Descending
                : !field.Descending;

            var part = desc
                ? builder.Descending(field.FieldFunc)
                : builder.Ascending(field.FieldFunc);

            sort = sort == null ? part : builder.Combine(sort, part);
        }

        return sort!;
    }
}