namespace DomnerTech.Backend.Application.Pagination.OffsetPaging;

/// <summary>
/// Defines the filter operators for query filtering.
/// </summary>
public enum FilterOperator
{
    /// <summary>
    /// Exact match (field = value).
    /// </summary>
    Equal,

    /// <summary>
    /// Not equal (field != value).
    /// </summary>
    NotEqual,

    /// <summary>
    /// Contains (for strings - case-insensitive).
    /// </summary>
    Contains,

    /// <summary>
    /// Starts with (for strings - case-insensitive).
    /// </summary>
    StartsWith,

    /// <summary>
    /// Ends with (for strings - case-insensitive).
    /// </summary>
    EndsWith,

    /// <summary>
    /// Greater than (field > value).
    /// </summary>
    GreaterThan,

    /// <summary>
    /// Greater than or equal (field >= value).
    /// </summary>
    GreaterThanOrEqual,

    /// <summary>
    /// Less than.
    /// </summary>
    LessThan,

    /// <summary>
    /// Less than or equal.
    /// </summary>
    LessThanOrEqual,

    /// <summary>
    /// In array (field in [value1, value2, ...]).
    /// </summary>
    In,

    /// <summary>
    /// Not in array (field not in [value1, value2, ...]).
    /// </summary>
    NotIn
}
