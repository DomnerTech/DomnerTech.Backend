using DomnerTech.Backend.Domain;
using System.Reflection;

namespace DomnerTech.Backend.Infrastructure.Pagination.OffsetPaging;

/// <summary>
/// Service for validating filterable and sortable fields for security.
/// Prevents SQL/NoSQL injection by ensuring only whitelisted fields are used.
/// </summary>
public sealed class FieldValidationService
{
    private readonly Dictionary<Type, HashSet<string>> _filterableFieldsCache = [];
    private readonly Dictionary<Type, HashSet<string>> _sortableFieldsCache = [];
    private readonly Lock _lock = new();

    /// <summary>
    /// Validates that a field is filterable on the given entity type.
    /// </summary>
    /// <typeparam name="T">The entity type.</typeparam>
    /// <param name="fieldName">The field name to validate.</param>
    /// <returns>True if the field is filterable, otherwise false.</returns>
    public bool IsFilterable<T>(string fieldName)
    {
        var filterableFields = GetFilterableFields<T>();
        return filterableFields.Contains(fieldName.ToLowerInvariant());
    }

    /// <summary>
    /// Validates that a field is sortable on the given entity type.
    /// </summary>
    /// <typeparam name="T">The entity type.</typeparam>
    /// <param name="fieldName">The field name to validate.</param>
    /// <returns>True if the field is sortable, otherwise false.</returns>
    public bool IsSortable<T>(string fieldName)
    {
        var sortableFields = GetSortableFields<T>();
        return sortableFields.Contains(fieldName.ToLowerInvariant());
    }

    /// <summary>
    /// Gets all filterable field names for the given entity type.
    /// </summary>
    /// <typeparam name="T">The entity type.</typeparam>
    /// <returns>Set of filterable field names (lowercase).</returns>
    public HashSet<string> GetFilterableFields<T>()
    {
        var type = typeof(T);

        lock (_lock)
        {
            if (_filterableFieldsCache.TryGetValue(type, out var cached))
            {
                return cached;
            }

            var fields = type
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(p => p.GetCustomAttribute<FilterableAttribute>() != null)
                .Select(p =>
                {
                    var attr = p.GetCustomAttribute<FilterableAttribute>();
                    return (attr?.Alias ?? p.Name).ToLowerInvariant();
                })
                .ToHashSet(StringComparer.OrdinalIgnoreCase);

            _filterableFieldsCache[type] = fields;
            return fields;
        }
    }

    /// <summary>
    /// Gets all sortable field names for the given entity type.
    /// </summary>
    /// <typeparam name="T">The entity type.</typeparam>
    /// <returns>Set of sortable field names (lowercase).</returns>
    public HashSet<string> GetSortableFields<T>()
    {
        var type = typeof(T);

        lock (_lock)
        {
            if (_sortableFieldsCache.TryGetValue(type, out var cached))
            {
                return cached;
            }

            var fields = type
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(p => p.GetCustomAttribute<SortableAttribute>() != null ||
                           p.GetCustomAttribute<FilterableAttribute>() != null)
                .Select(p =>
                {
                    var sortableAttr = p.GetCustomAttribute<SortableAttribute>();
                    var filterableAttr = p.GetCustomAttribute<FilterableAttribute>();
                    return (sortableAttr?.Alias ?? filterableAttr?.Alias ?? p.Name).ToLowerInvariant();
                })
                .ToHashSet(StringComparer.OrdinalIgnoreCase);

            _sortableFieldsCache[type] = fields;
            return fields;
        }
    }

    /// <summary>
    /// Validates multiple field names for filtering.
    /// </summary>
    /// <typeparam name="T">The entity type.</typeparam>
    /// <param name="fieldNames">The field names to validate.</param>
    /// <returns>List of invalid field names.</returns>
    public List<string> ValidateFilterableFields<T>(IEnumerable<string> fieldNames)
    {
        var filterableFields = GetFilterableFields<T>();
        return [.. fieldNames.Where(f => !filterableFields.Contains(f.ToLowerInvariant()))];
    }

    /// <summary>
    /// Validates multiple field names for sorting.
    /// </summary>
    /// <typeparam name="T">The entity type.</typeparam>
    /// <param name="fieldNames">The field names to validate.</param>
    /// <returns>List of invalid field names.</returns>
    public List<string> ValidateSortableFields<T>(IEnumerable<string> fieldNames)
    {
        var sortableFields = GetSortableFields<T>();
        return [.. fieldNames.Where(f => !sortableFields.Contains(f.TrimStart('-').ToLowerInvariant()))];
    }
}
