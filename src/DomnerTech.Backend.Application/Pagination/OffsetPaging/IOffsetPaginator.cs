using MongoDB.Driver;

namespace DomnerTech.Backend.Application.Pagination.OffsetPaging;

/// <summary>
/// Defines the contract for offset-based pagination operations.
/// </summary>
public interface IOffsetPaginator
{
    /// <summary>
    /// Applies pagination to a MongoDB query with filtering and sorting.
    /// </summary>
    /// <typeparam name="TEntity">The entity type to query.</typeparam>
    /// <typeparam name="TDto">The DTO type to return.</typeparam>
    /// <param name="collection">The MongoDB collection.</param>
    /// <param name="request">The pagination request.</param>
    /// <param name="baseFilter">Base filter to apply (e.g., tenant filter).</param>
    /// <param name="projection">Projection expression to map entity to DTO.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Paginated response with items and metadata.</returns>
    Task<OffsetPageResponse<TDto>> GetPageAsync<TEntity, TDto>(
        IMongoCollection<TEntity> collection,
        OffsetPageRequest request,
        FilterDefinition<TEntity>? baseFilter = null,
        Func<TEntity, TDto>? projection = null,
        CancellationToken cancellationToken = default)
        where TEntity : class
        where TDto : class;
}
