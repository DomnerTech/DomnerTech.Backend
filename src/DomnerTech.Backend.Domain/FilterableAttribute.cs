namespace DomnerTech.Backend.Domain;

/// <summary>
/// Marks a property as filterable in queries for security and validation.
/// Only properties marked with this attribute can be used in dynamic filters.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="FilterableAttribute"/> class.
/// </remarks>
/// <param name="alias">Optional alias for the field name.</param>
[AttributeUsage(AttributeTargets.Property)]
public sealed class FilterableAttribute(string? alias = null) : Attribute
{
    /// <summary>
    /// Gets the alias name to use for filtering (e.g., "name" instead of "Name").
    /// If null, the property name will be used.
    /// </summary>
    public string? Alias { get; } = alias;

    /// <summary>
    /// Gets whether exact match filtering is allowed.
    /// </summary>
    public bool AllowExactMatch { get; init; } = true;

    /// <summary>
    /// Gets whether partial/contains filtering is allowed (for strings).
    /// </summary>
    public bool AllowPartialMatch { get; init; } = true;

    /// <summary>
    /// Gets whether range filtering is allowed (>, <, >=, <=).
    /// </summary>
    public bool AllowRangeFilter { get; init; } = true;
}
