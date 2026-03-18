namespace DomnerTech.Backend.Application.Pagination.OffsetPaging;

/// <summary>
/// Defines the filter operators for query filtering.
/// </summary>
public enum FilterOperator
{
    /// <summary>
    /// Exact match (field = value).
    /// </summary>
    Eq,

    /// <summary>
    /// Not equal (field != value).
    /// </summary>
    Ne,

    /// <summary>
    /// Contains (for strings - case-insensitive).
    /// </summary>
    Ct,

    /// <summary>
    /// Starts with (for strings - case-insensitive).
    /// </summary>
    Sw,

    /// <summary>
    /// Ends with (for strings - case-insensitive).
    /// </summary>
    Ew,

    /// <summary>
    /// Greater than (field > value).
    /// </summary>
    Gt,

    /// <summary>
    /// Greater than or equal (field >= value).
    /// </summary>
    Gte,

    /// <summary>
    /// Less than.
    /// </summary>
    Lt,

    /// <summary>
    /// Less than or equal.
    /// </summary>
    Lte,

    /// <summary>
    /// In array (field in [value1, value2, ...]).
    /// </summary>
    In,

    /// <summary>
    /// Not in array (field not in [value1, value2, ...]).
    /// </summary>
    Nin
}
