namespace DomnerTech.Backend.Application.Pagination.OffsetPaging;

/// <summary>
/// Defines the sort order direction.
/// </summary>
public enum SortOrder
{
    /// <summary>
    /// Ascending order (A-Z, 0-9, oldest-newest).
    /// </summary>
    Ascending,

    /// <summary>
    /// Descending order (Z-A, 9-0, newest-oldest).
    /// </summary>
    Descending
}
