namespace DomnerTech.Backend.Application.Pagination.OffsetPaging;

/// <summary>
/// Represents a single field filter for querying.
/// </summary>
public sealed class FieldFilter
{
    /// <summary>
    /// Gets or sets the field name to filter on.
    /// </summary>
    public required string Field { get; set; }

    /// <summary>
    /// Gets or sets the filter operator.
    /// </summary>
    public FilterOperator Operator { get; set; } = FilterOperator.Eq;

    /// <summary>
    /// Gets or sets the value to filter by.
    /// Can be a single value or comma-separated values for In/NotIn operators.
    /// </summary>
    public required string Value { get; set; }
}
