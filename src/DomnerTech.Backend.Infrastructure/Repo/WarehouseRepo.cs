using DomnerTech.Backend.Application.Constants;
using DomnerTech.Backend.Application.IRepo;
using DomnerTech.Backend.Application.Services;
using DomnerTech.Backend.Domain.Entities;
using DomnerTech.Backend.Infrastructure.MongoDb;
using MongoDB.Bson;
using MongoDB.Driver;

namespace DomnerTech.Backend.Infrastructure.Repo;

/// <summary>
/// Repository implementation for Warehouse entity.
/// </summary>
public sealed class WarehouseRepo(
    IMongoDbContextFactory contextFactory,
    ITenantService tenant)
    : BaseRepo<WarehouseEntity>(contextFactory.Create(DatabaseNameConstant.DatabaseName).Database, tenant), IWarehouseRepo
{
    public async Task<ObjectId> CreateAsync(WarehouseEntity entity, CancellationToken cancellationToken = default)
    {
        await Collection.InsertOneAsync(entity, cancellationToken: cancellationToken);
        return entity.Id;
    }

    public async Task UpdateAsync(WarehouseEntity entity, CancellationToken cancellationToken = default)
    {
        var filter = Builders<WarehouseEntity>.Filter.Eq(x => x.Id, entity.Id);
        await Collection.ReplaceOneAsync(TenantFilter() & filter, entity, cancellationToken: cancellationToken);
    }

    public async Task<WarehouseEntity?> GetByIdAsync(ObjectId id, CancellationToken cancellationToken = default)
    {
        var filter = Builders<WarehouseEntity>.Filter.And(
            Builders<WarehouseEntity>.Filter.Eq(x => x.Id, id),
            Builders<WarehouseEntity>.Filter.Eq(x => x.IsDeleted, false)
        );
        return await Collection.Find(TenantFilter() & filter).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<WarehouseEntity?> GetByCodeAsync(string code, CancellationToken cancellationToken = default)
    {
        var filter = Builders<WarehouseEntity>.Filter.And(
            Builders<WarehouseEntity>.Filter.Eq(x => x.Code, code),
            Builders<WarehouseEntity>.Filter.Eq(x => x.IsDeleted, false)
        );
        return await Collection.Find(TenantFilter() & filter).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<List<WarehouseEntity>> GetAllActiveAsync(CancellationToken cancellationToken = default)
    {
        var filter = Builders<WarehouseEntity>.Filter.And(
            Builders<WarehouseEntity>.Filter.Eq(x => x.IsActive, true),
            Builders<WarehouseEntity>.Filter.Eq(x => x.IsDeleted, false)
        );
        return await Collection.Find(TenantFilter() & filter)
            .SortBy(x => x.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<WarehouseEntity?> GetDefaultAsync(CancellationToken cancellationToken = default)
    {
        var filter = Builders<WarehouseEntity>.Filter.And(
            Builders<WarehouseEntity>.Filter.Eq(x => x.IsDefault, true),
            Builders<WarehouseEntity>.Filter.Eq(x => x.IsActive, true),
            Builders<WarehouseEntity>.Filter.Eq(x => x.IsDeleted, false)
        );
        return await Collection.Find(TenantFilter() & filter).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task DeleteAsync(ObjectId id, ObjectId deletedBy, CancellationToken cancellationToken = default)
    {
        var filter = Builders<WarehouseEntity>.Filter.Eq(x => x.Id, id);
        var update = Builders<WarehouseEntity>.Update
            .Set(x => x.IsDeleted, true)
            .Set(x => x.DeletedBy, deletedBy)
            .Set(x => x.UpdatedAt, DateTime.UtcNow);
        
        await Collection.UpdateOneAsync(TenantFilter() & filter, update, cancellationToken: cancellationToken);
    }
}
