namespace DomnerTech.Backend.Application.Pagination.OffsetPaging;

/// <summary>
/// Request model for offset-based pagination with sorting and filtering.
/// </summary>
public class OffsetPageRequest
{
    /// <summary>
    /// Gets or sets the page number (1-based).
    /// Minimum: 1, Maximum: 1000.
    /// </summary>
    public int PageNumber
    {
        get;
        set
        {
            field = value switch
            {
                >= 1 and <= 1000 => value,
                < 1 => 1,
                _ => 1000
            };
        }
    } = 1;

    /// <summary>
    /// Gets or sets the page size (number of items per page).
    /// Minimum: 1, Maximum: 100.
    /// </summary>
    public int PageSize
    {
        get;
        set => field = value switch
        {
            >= 1 and <= 100 => value,
            < 1 => 1,
            _ => 100
        };
    } = 20;

    /// <summary>
    /// Gets or sets the sort expression.
    /// Format: "fieldName" for ascending, "-fieldName" for descending.
    /// Multiple fields: "name,-createdAt,price".
    /// </summary>
    public string? Sort { get; set; }

    /// <summary>
    /// Gets or sets the filters to apply.
    /// </summary>
    public List<FieldFilter>? Filters { get; set; }

    /// <summary>
    /// Gets or sets whether to include the total count of records.
    /// Default: true. Set to false for better performance when count is not needed.
    /// </summary>
    public bool IncludeTotalCount { get; set; } = true;

    /// <summary>
    /// Gets the calculated skip value for MongoDB.
    /// </summary>
    public int Skip => (PageNumber - 1) * PageSize;
}
