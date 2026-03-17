namespace DomnerTech.Backend.Application.Pagination.OffsetPaging;

/// <summary>
/// Generic response model for offset-based pagination.
/// </summary>
/// <typeparam name="T">The type of items in the page.</typeparam>
public sealed class OffsetPageResponse<T>
{
    /// <summary>
    /// Gets or sets the items in the current page.
    /// </summary>
    public required IReadOnlyList<T> Items { get; set; }

    /// <summary>
    /// Gets or sets the total count of items across all pages.
    /// Null if IncludeTotalCount was false in the request.
    /// </summary>
    public long? TotalCount { get; set; }

    /// <summary>
    /// Gets or sets the current page number.
    /// </summary>
    public int PageNumber { get; set; }

    /// <summary>
    /// Gets or sets the page size.
    /// </summary>
    public int PageSize { get; set; }

    /// <summary>
    /// Gets the total number of pages.
    /// Null if TotalCount is null.
    /// </summary>
    public int? TotalPages => TotalCount.HasValue
        ? (int)Math.Ceiling(TotalCount.Value / (double)PageSize)
        : null;

    /// <summary>
    /// Gets whether there is a next page.
    /// </summary>
    public bool HasNextPage => TotalCount.HasValue && PageNumber < TotalPages;

    /// <summary>
    /// Gets whether there is a previous page.
    /// </summary>
    public bool HasPreviousPage => PageNumber > 1;
}
