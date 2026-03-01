namespace DomnerTech.Backend.Application.Pagination;

public sealed class KeysetPageResult<T>
{
    public IReadOnlyList<T> Items { get; init; } = [];
    public string? NextCursor { get; init; }
    public string? PreviousCursor { get; init; }
    public bool HasNextPage { get; init; }
    public bool HasPreviousPage { get; init; }
    public long? TotalItems { get; init; }
}

public static class KeysetPageResultExtensions
{
    public static KeysetPageResult<TDto> ToDto<TEntity, TDto>(
        this KeysetPageResult<TEntity> source,
        Func<TEntity, TDto> mapper)
    {
        return new KeysetPageResult<TDto>
        {
            HasNextPage = source.HasNextPage,
            HasPreviousPage = source.HasPreviousPage,
            Items = source.Items.Select(mapper).ToList().AsReadOnly(),
            NextCursor = source.NextCursor,
            PreviousCursor = source.PreviousCursor,
            TotalItems = source.TotalItems
        };
    }
}