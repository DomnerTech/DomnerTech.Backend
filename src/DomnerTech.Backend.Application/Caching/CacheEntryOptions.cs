namespace DomnerTech.Backend.Application.Caching;

public class CacheEntryOptions
{
    public DateTimeOffset? AbsoluteExpiration { get; set; }
    public TimeSpan? SlidingExpiration { get; set; }
}