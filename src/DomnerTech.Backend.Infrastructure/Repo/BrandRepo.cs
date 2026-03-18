using System.Linq.Expressions;
using DomnerTech.Backend.Application.Constants;
using DomnerTech.Backend.Application.DTOs.Brands;
using DomnerTech.Backend.Application.IRepo;
using DomnerTech.Backend.Application.Pagination.OffsetPaging;
using DomnerTech.Backend.Application.Services;
using DomnerTech.Backend.Domain.Entities;
using DomnerTech.Backend.Infrastructure.MongoDb;
using MongoDB.Bson;
using MongoDB.Driver;

namespace DomnerTech.Backend.Infrastructure.Repo;

/// <summary>
/// Repository implementation for Brand entity.
/// </summary>
public sealed class BrandRepo(
    IMongoDbContextFactory contextFactory,
    ITenantService tenant,
    IOffsetPaginator<BrandEntity, BrandDto> paginator)
    : BaseRepo<BrandEntity>(contextFactory.Create(DatabaseNameConstant.DatabaseName).Database, tenant), IBrandRepo
{
    public async Task<ObjectId> CreateAsync(BrandEntity entity, CancellationToken cancellationToken = default)
    {
        await Collection.InsertOneAsync(entity, cancellationToken: cancellationToken);
        return entity.Id;
    }

    public async Task UpdateAsync(BrandEntity entity, CancellationToken cancellationToken = default)
    {
        var filter = Builders<BrandEntity>.Filter.Eq(x => x.Id, entity.Id);
        await Collection.ReplaceOneAsync(TenantFilter() & filter, entity, cancellationToken: cancellationToken);
    }

    public async Task<BrandEntity?> GetByIdAsync(ObjectId id, CancellationToken cancellationToken = default)
    {
        var filter = Builders<BrandEntity>.Filter.Eq(x => x.Id, id);
        return await Collection.Find(TenantFilter() & filter).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<List<BrandEntity>> GetAllActiveAsync(CancellationToken cancellationToken = default)
    {
        var filter = Builders<BrandEntity>.Filter.Eq(x => x.IsActive, true);
        return await Collection.Find(TenantFilter() & filter).ToListAsync(cancellationToken);
    }

    public async Task<OffsetPageResponse<BrandDto>> GetPagedAsync(
        OffsetPageRequest request,
        Expression<Func<BrandEntity, BrandDto>>? projection = null,
        CancellationToken cancellationToken = default)
    {
        var paged = await paginator.GetPageAsync(
            collection: Collection,
            request: request,
            baseFilter: TenantFilter(),
            projection: projection,
            cancellationToken: cancellationToken);
        return paged;
    }
}
