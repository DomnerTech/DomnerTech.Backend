using DomnerTech.Backend.Application.Constants;
using DomnerTech.Backend.Application.IRepo;
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
    ITenantService tenant)
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
        var filter = Builders<BrandEntity>.Filter.And(
            Builders<BrandEntity>.Filter.Eq(x => x.Id, id),
            Builders<BrandEntity>.Filter.Eq(x => x.IsDeleted, false)
        );
        return await Collection.Find(TenantFilter() & filter).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<BrandEntity?> GetBySlugAsync(string slug, CancellationToken cancellationToken = default)
    {
        var filter = Builders<BrandEntity>.Filter.And(
            Builders<BrandEntity>.Filter.Eq(x => x.Slug, slug),
            Builders<BrandEntity>.Filter.Eq(x => x.IsDeleted, false)
        );
        return await Collection.Find(TenantFilter() & filter).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<List<BrandEntity>> GetAllActiveAsync(CancellationToken cancellationToken = default)
    {
        var filter = Builders<BrandEntity>.Filter.And(
            Builders<BrandEntity>.Filter.Eq(x => x.IsActive, true),
            Builders<BrandEntity>.Filter.Eq(x => x.IsDeleted, false)
        );
        return await Collection.Find(TenantFilter() & filter)
            .SortBy(x => x.DisplayOrder)
            .ThenBy(x => x.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task DeleteAsync(ObjectId id, ObjectId deletedBy, CancellationToken cancellationToken = default)
    {
        var filter = Builders<BrandEntity>.Filter.Eq(x => x.Id, id);
        var update = Builders<BrandEntity>.Update
            .Set(x => x.IsDeleted, true)
            .Set(x => x.DeletedBy, deletedBy)
            .Set(x => x.UpdatedAt, DateTime.UtcNow);
        
        await Collection.UpdateOneAsync(TenantFilter() & filter, update, cancellationToken: cancellationToken);
    }
}
