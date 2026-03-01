namespace DomnerTech.Backend.Application.Pagination;

public class KeysetPageRequest
{
    public string? Cursor { get; set; }
    public int PageSize { get; set; } = 20;
    public CursorDirection Direction { get; set; } = CursorDirection.Forward;
    public bool IncludeTotalCount { get; set; }
    public string SortKey { get; init; } = "id";
}