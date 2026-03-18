using System.Linq.Expressions;
using DomnerTech.Backend.Application.DTOs;
using DomnerTech.Backend.Application.Pagination.OffsetPaging;
using DomnerTech.Backend.Domain.Entities;
using MongoDB.Driver;

namespace DomnerTech.Backend.Infrastructure.Pagination.OffsetPaging;

/// <summary>
/// MongoDB implementation of offset-based pagination.
/// Optimized for high-performance queries on large datasets.
/// </summary>
public sealed class MongoOffsetPaginator<TEntity, TDto> : IOffsetPaginator<TEntity, TDto>
    where TEntity : IBaseEntity
    where TDto : IBaseDto
{
    /// <summary>
    /// Applies pagination to a MongoDB query with filtering and sorting.
    /// </summary>
    /// <typeparam name="TEntity">The entity type to query.</typeparam>
    /// <typeparam name="TDto">The DTO type to return.</typeparam>
    /// <param name="collection">The MongoDB collection.</param>
    /// <param name="request">The pagination request.</param>
    /// <param name="baseFilter">Base filter to apply (e.g., tenant filter, soft delete filter).</param>
    /// <param name="projection">Projection function to map entity to DTO. If null, TEntity must be TDto.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Paginated response with items and metadata.</returns>
    public async Task<OffsetPageResponse<TDto>> GetPageAsync(
        IMongoCollection<TEntity> collection,
        OffsetPageRequest request,
        FilterDefinition<TEntity>? baseFilter = null,
        Expression<Func<TEntity, TDto>>? projection = null,
        CancellationToken cancellationToken = default)
    {
        // Start with base filter or empty filter
        var filter = baseFilter ?? Builders<TEntity>.Filter.Empty;

        // Apply dynamic filtering from request
        filter = filter.ApplyFiltering(request.Filters);

        // Build the query with sorting
        var query = collection
            .Find(filter)
            .ApplySorting(request.Sort);

        // Get total count if requested (parallel execution for better performance)
        Task<long>? countTask = null;
        if (request.IncludeTotalCount)
        {
            countTask = collection.CountDocumentsAsync(filter, cancellationToken: cancellationToken);
        }

        // Apply pagination and execute query
        var entities = await query
            .ApplyPagination(request)
            .ToListAsync(cancellationToken);

        // Project entities to DTOs
        List<TDto> items;
        if (projection != null)
        {
            items = [.. entities.Select(projection.Compile())];
        }
        else if (typeof(TEntity) == typeof(TDto))
        {
            // ReSharper disable once SuspiciousTypeConversion.Global
            items = [.. entities.Cast<TDto>()];
        }
        else
        {
            throw new InvalidOperationException(
                $"Projection function is required when TEntity ({typeof(TEntity).Name}) differs from TDto ({typeof(TDto).Name})");
        }

        // Await count if it was requested
        long? totalCount = null;
        if (countTask != null)
        {
            totalCount = await countTask;
        }

        return new OffsetPageResponse<TDto>
        {
            Items = items.AsReadOnly(),
            TotalCount = totalCount,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize
        };
    }
}
