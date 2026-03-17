using DomnerTech.Backend.Application.Pagination.OffsetPaging;

namespace DomnerTech.Backend.Application.IRepo;

public interface IBaseRepo;

/// <summary>
/// Base repository interface with common data access operations.
/// </summary>
public interface IBasePagedRepo
{
    /// <summary>
    /// Gets a paginated list of items with filtering and sorting support.
    /// </summary>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    /// <typeparam name="TDto">The DTO type to return.</typeparam>
    /// <param name="request">The pagination request.</param>
    /// <param name="projection">Optional projection function to map entity to DTO.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Paginated response with items and metadata.</returns>
    Task<OffsetPageResponse<TDto>> GetPagedAsync<TEntity, TDto>(
        OffsetPageRequest request,
        Func<TEntity, TDto>? projection = null,
        CancellationToken cancellationToken = default)
        where TEntity : class
        where TDto : class;
}

