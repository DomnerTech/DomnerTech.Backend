using DomnerTech.Backend.Application.Extensions;
using DomnerTech.Backend.Application.Services;
using MongoDB.Driver;

namespace DomnerTech.Backend.Infrastructure.Repo;

public abstract class BaseRepo<T>(
    IMongoDatabase database,
    ITenantService tenant) where T : class
{
    protected readonly IMongoCollection<T> Collection = database.GetCollection<T>();
    protected readonly ITenantService Tenant = tenant;

    protected FilterDefinition<T> TenantFilter()
    {
        return Builders<T>.Filter.Eq("companyId", Tenant.CompanyId.ToObjectId());
    }

    /// <summary>
    /// Gets a paginated list of items with filtering and sorting support.
    /// Automatically applies tenant filtering.
    /// </summary>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    /// <typeparam name="TDto">The DTO type to return.</typeparam>
    /// <param name="request">The pagination request.</param>
    /// <param name="projection">Optional projection function to map entity to DTO.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Paginated response with items and metadata.</returns>
    //public virtual async Task<OffsetPageResponse<TDto>> GetPagedAsync<TEntity, TDto>(
    //    OffsetPageRequest request,
    //    Func<TEntity, TDto>? projection = null,
    //    CancellationToken cancellationToken = default)
    //    where TEntity : class
    //    where TDto : class
    //{
    //    var collection = database.GetCollection<TEntity>();

    //    // Build base filter (tenant filter if entity supports multi-tenancy)
    //    FilterDefinition<TEntity>? baseFilter = null;

    //    // Check if entity implements ITenantEntity
    //    if (typeof(ITenantEntity).IsAssignableFrom(typeof(TEntity)))
    //    {
    //        baseFilter = Builders<TEntity>.Filter.Eq("companyId", Tenant.CompanyId.ToObjectId());
    //    }

    //    return await Paginator.GetPageAsync(
    //        collection,
    //        request,
    //        baseFilter,
    //        projection,
    //        cancellationToken);
    //}
}