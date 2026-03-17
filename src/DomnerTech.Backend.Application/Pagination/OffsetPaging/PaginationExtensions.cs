using MongoDB.Driver;
using System.Reflection;
using DomnerTech.Backend.Domain;

namespace DomnerTech.Backend.Application.Pagination.OffsetPaging;

/// <summary>
/// Extension methods for MongoDB pagination operations.
/// </summary>
public static class PaginationExtensions
{
    /// <summary>
    /// Applies dynamic filtering to a MongoDB filter definition based on the request filters.
    /// </summary>
    /// <typeparam name="T">The entity type.</typeparam>
    /// <param name="baseFilter">The base filter to extend.</param>
    /// <param name="filters">The list of field filters to apply.</param>
    /// <returns>Combined filter definition.</returns>
    public static FilterDefinition<T> ApplyFiltering<T>(
        this FilterDefinition<T> baseFilter,
        List<FieldFilter>? filters) where T : class
    {
        if (filters == null || filters.Count == 0)
            return baseFilter;

        var filterBuilder = Builders<T>.Filter;
        var additionalFilters = new List<FilterDefinition<T>>();

        // Get all filterable properties with their metadata
        var filterableProperties = GetFilterableProperties<T>();

        foreach (var filter in filters)
        {
            // Validate that the field is allowed
            if (!filterableProperties.TryGetValue(filter.Field.ToLowerInvariant(), out var propInfo))
            {
                throw new InvalidOperationException($"Field '{filter.Field}' is not filterable on type {typeof(T).Name}");
            }

            var (property, attribute) = propInfo;
            var fieldName = GetMongoFieldName(property);

            // Build the appropriate filter based on operator
            var fieldFilter = BuildFieldFilter<T>(filterBuilder, fieldName, filter, property.PropertyType, attribute);
            if (fieldFilter != null)
            {
                additionalFilters.Add(fieldFilter);
            }
        }

        // Combine all filters
        if (additionalFilters.Count > 0)
        {
            return filterBuilder.And(baseFilter, filterBuilder.And(additionalFilters));
        }

        return baseFilter;
    }

    /// <summary>
    /// Applies dynamic sorting to a MongoDB find fluent based on the sort expression.
    /// </summary>
    /// <typeparam name="T">The entity type.</typeparam>
    /// <param name="findFluent">The find fluent to sort.</param>
    /// <param name="sortExpression">Sort expression (e.g., "name,-createdAt").</param>
    /// <returns>Sorted find fluent.</returns>
    public static IFindFluent<T, T> ApplySorting<T>(
        this IFindFluent<T, T> findFluent,
        string? sortExpression) where T : class
    {
        if (string.IsNullOrWhiteSpace(sortExpression))
        {
            // Default sort by _id descending
            return findFluent.SortByDescending(x => x);
        }

        var sortFields = sortExpression.Split(',', StringSplitOptions.RemoveEmptyEntries);
        if (sortFields.Length == 0)
        {
            return findFluent.SortByDescending(x => x);
        }

        // Get sortable properties
        var sortableProperties = GetSortableProperties<T>();
        SortDefinition<T>? sortDefinition = null;

        foreach (var field in sortFields)
        {
            var trimmedField = field.Trim();
            var isDescending = trimmedField.StartsWith('-');
            var fieldName = isDescending ? trimmedField[1..] : trimmedField;

            // Validate field is sortable
            if (!sortableProperties.TryGetValue(fieldName.ToLowerInvariant(), out var property))
            {
                throw new InvalidOperationException($"Field '{fieldName}' is not sortable on type {typeof(T).Name}");
            }

            var mongoFieldName = GetMongoFieldName(property);
            var fieldSort = isDescending
                ? Builders<T>.Sort.Descending(mongoFieldName)
                : Builders<T>.Sort.Ascending(mongoFieldName);

            sortDefinition = sortDefinition == null
                ? fieldSort
                : Builders<T>.Sort.Combine(sortDefinition, fieldSort);
        }

        return sortDefinition != null ? findFluent.Sort(sortDefinition) : findFluent;
    }

    /// <summary>
    /// Applies pagination (skip and limit) to a MongoDB find fluent.
    /// </summary>
    /// <typeparam name="T">The entity type.</typeparam>
    /// <param name="findFluent">The find fluent to paginate.</param>
    /// <param name="request">The pagination request.</param>
    /// <returns>Paginated find fluent.</returns>
    public static IFindFluent<T, T> ApplyPagination<T>(
        this IFindFluent<T, T> findFluent,
        OffsetPageRequest request) where T : class
    {
        return findFluent
            .Skip(request.Skip)
            .Limit(request.PageSize);
    }

    /// <summary>
    /// Gets all properties marked with FilterableAttribute.
    /// </summary>
    private static Dictionary<string, (PropertyInfo Property, FilterableAttribute Attribute)> GetFilterableProperties<T>()
    {
        return typeof(T)
            .GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Select(p => new
            {
                Property = p,
                Attribute = p.GetCustomAttribute<FilterableAttribute>()
            })
            .Where(x => x.Attribute != null)
            .ToDictionary(
                x => (x.Attribute!.Alias ?? x.Property.Name).ToLowerInvariant(),
                x => (x.Property, x.Attribute!),
                StringComparer.OrdinalIgnoreCase
            );
    }

    /// <summary>
    /// Gets all properties marked with SortableAttribute or FilterableAttribute.
    /// </summary>
    private static Dictionary<string, PropertyInfo> GetSortableProperties<T>()
    {
        return typeof(T)
            .GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Select(p => new
            {
                Property = p,
                SortableAttr = p.GetCustomAttribute<SortableAttribute>(),
                FilterableAttr = p.GetCustomAttribute<FilterableAttribute>()
            })
            .Where(x => x.SortableAttr != null || x.FilterableAttr != null)
            .ToDictionary(
                x => (x.SortableAttr?.Alias ?? x.FilterableAttr?.Alias ?? x.Property.Name).ToLowerInvariant(),
                x => x.Property,
                StringComparer.OrdinalIgnoreCase
            );
    }

    /// <summary>
    /// Builds a filter definition for a specific field based on the operator.
    /// </summary>
    private static FilterDefinition<T>? BuildFieldFilter<T>(
        FilterDefinitionBuilder<T> builder,
        string fieldName,
        FieldFilter filter,
        Type propertyType,
        FilterableAttribute attribute) where T : class
    {
        try
        {
            return filter.Operator switch
            {
                FilterOperator.Equal => BuildEqualFilter(builder, fieldName, filter.Value, propertyType, attribute),
                FilterOperator.NotEqual => BuildNotEqualFilter(builder, fieldName, filter.Value, propertyType, attribute),
                FilterOperator.Contains => BuildContainsFilter(builder, fieldName, filter.Value, attribute),
                FilterOperator.StartsWith => BuildStartsWithFilter(builder, fieldName, filter.Value, attribute),
                FilterOperator.EndsWith => BuildEndsWithFilter(builder, fieldName, filter.Value, attribute),
                FilterOperator.GreaterThan => BuildComparisonFilter(builder, fieldName, filter.Value, propertyType, attribute, ">"),
                FilterOperator.GreaterThanOrEqual => BuildComparisonFilter(builder, fieldName, filter.Value, propertyType, attribute, ">="),
                FilterOperator.LessThan => BuildComparisonFilter(builder, fieldName, filter.Value, propertyType, attribute, "<"),
                FilterOperator.LessThanOrEqual => BuildComparisonFilter(builder, fieldName, filter.Value, propertyType, attribute, "<="),
                FilterOperator.In => BuildInFilter(builder, fieldName, filter.Value, propertyType, attribute),
                FilterOperator.NotIn => BuildNotInFilter(builder, fieldName, filter.Value, propertyType, attribute),
                _ => null
            };
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException(
                $"Failed to build filter for field '{filter.Field}' with operator '{filter.Operator}': {ex.Message}",
                ex);
        }
    }

    private static FilterDefinition<T> BuildEqualFilter<T>(
        FilterDefinitionBuilder<T> builder,
        string fieldName,
        string value,
        Type propertyType,
        FilterableAttribute attribute) where T : class
    {
        if (!attribute.AllowExactMatch)
            throw new InvalidOperationException($"Exact match filtering is not allowed on field '{fieldName}'");

        var convertedValue = ConvertValue(value, propertyType);
        return builder.Eq(fieldName, convertedValue);
    }

    private static FilterDefinition<T> BuildNotEqualFilter<T>(
        FilterDefinitionBuilder<T> builder,
        string fieldName,
        string value,
        Type propertyType,
        FilterableAttribute attribute) where T : class
    {
        if (!attribute.AllowExactMatch)
            throw new InvalidOperationException($"Not equal filtering is not allowed on field '{fieldName}'");

        var convertedValue = ConvertValue(value, propertyType);
        return builder.Ne(fieldName, convertedValue);
    }

    private static FilterDefinition<T> BuildContainsFilter<T>(
        FilterDefinitionBuilder<T> builder,
        string fieldName,
        string value,
        FilterableAttribute attribute) where T : class
    {
        if (!attribute.AllowPartialMatch)
            throw new InvalidOperationException($"Contains filtering is not allowed on field '{fieldName}'");

        return builder.Regex(fieldName, new MongoDB.Bson.BsonRegularExpression(value, "i"));
    }

    private static FilterDefinition<T> BuildStartsWithFilter<T>(
        FilterDefinitionBuilder<T> builder,
        string fieldName,
        string value,
        FilterableAttribute attribute) where T : class
    {
        if (!attribute.AllowPartialMatch)
            throw new InvalidOperationException($"StartsWith filtering is not allowed on field '{fieldName}'");

        return builder.Regex(fieldName, new MongoDB.Bson.BsonRegularExpression($"^{value}", "i"));
    }

    private static FilterDefinition<T> BuildEndsWithFilter<T>(
        FilterDefinitionBuilder<T> builder,
        string fieldName,
        string value,
        FilterableAttribute attribute) where T : class
    {
        if (!attribute.AllowPartialMatch)
            throw new InvalidOperationException($"EndsWith filtering is not allowed on field '{fieldName}'");

        return builder.Regex(fieldName, new MongoDB.Bson.BsonRegularExpression($"{value}$", "i"));
    }

    private static FilterDefinition<T> BuildComparisonFilter<T>(
        FilterDefinitionBuilder<T> builder,
        string fieldName,
        string value,
        Type propertyType,
        FilterableAttribute attribute,
        string operation) where T : class
    {
        if (!attribute.AllowRangeFilter)
            throw new InvalidOperationException($"Range filtering is not allowed on field '{fieldName}'");

        var convertedValue = ConvertValue(value, propertyType);

        return operation switch
        {
            ">" => builder.Gt(fieldName, convertedValue),
            ">=" => builder.Gte(fieldName, convertedValue),
            "<" => builder.Lt(fieldName, convertedValue),
            "<=" => builder.Lte(fieldName, convertedValue),
            _ => throw new NotSupportedException($"Comparison operator '{operation}' is not supported")
        };
    }

    private static FilterDefinition<T> BuildInFilter<T>(
        FilterDefinitionBuilder<T> builder,
        string fieldName,
        string value,
        Type propertyType,
        FilterableAttribute attribute) where T : class
    {
        if (!attribute.AllowExactMatch)
            throw new InvalidOperationException($"In filtering is not allowed on field '{fieldName}'");

        var values = value.Split(',', StringSplitOptions.RemoveEmptyEntries)
            .Select(v => ConvertValue(v.Trim(), propertyType))
            .ToArray();

        return builder.In(fieldName, values);
    }

    private static FilterDefinition<T> BuildNotInFilter<T>(
        FilterDefinitionBuilder<T> builder,
        string fieldName,
        string value,
        Type propertyType,
        FilterableAttribute attribute) where T : class
    {
        if (!attribute.AllowExactMatch)
            throw new InvalidOperationException($"NotIn filtering is not allowed on field '{fieldName}'");

        var values = value.Split(',', StringSplitOptions.RemoveEmptyEntries)
            .Select(v => ConvertValue(v.Trim(), propertyType))
            .ToArray();

        return builder.Nin(fieldName, values);
    }

    /// <summary>
    /// Converts a string value to the target property type.
    /// </summary>
    private static object ConvertValue(string value, Type targetType)
    {
        try
        {
            // Handle nullable types
            var underlyingType = Nullable.GetUnderlyingType(targetType) ?? targetType;

            // Handle enums
            if (underlyingType.IsEnum)
            {
                return Enum.Parse(underlyingType, value, true);
            }

            // Handle MongoDB ObjectId
            if (underlyingType == typeof(MongoDB.Bson.ObjectId))
            {
                return MongoDB.Bson.ObjectId.Parse(value);
            }

            // Handle common types
            return underlyingType.Name switch
            {
                nameof(String) => value,
                nameof(Int32) => int.Parse(value),
                nameof(Int64) => long.Parse(value),
                nameof(Decimal) => decimal.Parse(value),
                nameof(Double) => double.Parse(value),
                nameof(Boolean) => bool.Parse(value),
                nameof(DateTime) => DateTime.Parse(value),
                nameof(DateTimeOffset) => DateTimeOffset.Parse(value),
                nameof(Guid) => Guid.Parse(value),
                _ => Convert.ChangeType(value, underlyingType)
            };
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException(
                $"Failed to convert value '{value}' to type {targetType.Name}: {ex.Message}",
                ex);
        }
    }

    /// <summary>
    /// Gets the MongoDB field name for a property (converts to camelCase).
    /// </summary>
    private static string GetMongoFieldName(PropertyInfo property)
    {
        // MongoDB typically uses camelCase, but respect BsonElement if present
        var bsonElement = property.GetCustomAttribute<MongoDB.Bson.Serialization.Attributes.BsonElementAttribute>();
        if (bsonElement != null && !string.IsNullOrWhiteSpace(bsonElement.ElementName))
        {
            return bsonElement.ElementName;
        }

        // Convert to camelCase
        var name = property.Name;
        return char.ToLowerInvariant(name[0]) + name[1..];
    }
}
