using DomnerTech.Backend.Domain;
using System.Reflection;
using DomnerTech.Backend.Application.Exceptions;

namespace DomnerTech.Backend.Application.Pagination;

public class SortProfile<T>
{
    private static readonly Lazy<Dictionary<string, IReadOnlyList<CursorField<T>>>> SMap
        = new(BuildMap, isThreadSafe: true);

    private const string Id = "Id";

    private static Dictionary<string, IReadOnlyList<CursorField<T>>> BuildMap()
    {
        var map = new Dictionary<string, IReadOnlyList<CursorField<T>>>(StringComparer.OrdinalIgnoreCase);

        var properties = typeof(T)
            .GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Select(p => new
            {
                Property = p,
                Attribute = p.GetCustomAttribute<SortableAttribute>()
            })
            .Where(x => x.Attribute != null)
            .ToList();

        var grouped = properties
            .GroupBy(x => x.Attribute!.Alias ?? x.Property.Name);

        foreach (var group in grouped)
        {
            var fields = group
                .OrderBy(x => x.Attribute!.Order)
                .Select(x => CursorField<T>.FromProperty(x.Property, x.Attribute!.Descending))
                .ToList();

            var idProp = typeof(T).GetProperty(Id, BindingFlags.Public | BindingFlags.Instance);
            if (idProp != null && fields.All(i => i.GetFieldName() != Id))
            {
                fields.Add(CursorField<T>.FromProperty(idProp));
            }

            map[group.Key] = fields;
        }

        // Optional default sort if none defined
        if (map.Count == 0 && typeof(T).GetProperty(Id) != null)
        {
            map["default"] =
            [
                new CursorField<T>(i => Id)
            ];
        }

        return map;
    }

    public IReadOnlyList<CursorField<T>> Resolve(string key)
    {
        var keys = key.Split(',');
        var multiFields = new List<CursorField<T>>();
        foreach (var k in keys)
        {
            if (SMap.Value.TryGetValue(k.Trim(), out var fields))
            {
                multiFields.AddRange(fields);
            }
            else
            {
                throw new InvalidSortKeyException(k);
            }
        }

        return multiFields;
    }
}