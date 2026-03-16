using DomnerTech.Backend.Application.Constants;
using DomnerTech.Backend.Application.IRepo;
using DomnerTech.Backend.Application.Services;
using DomnerTech.Backend.Domain.Entities;
using DomnerTech.Backend.Domain.Enums;
using DomnerTech.Backend.Infrastructure.MongoDb;
using MongoDB.Bson;
using MongoDB.Driver;

namespace DomnerTech.Backend.Infrastructure.Repo;

/// <summary>
/// Repository implementation for StockMovement entity.
/// </summary>
public sealed class StockMovementRepo(
    IMongoDbContextFactory contextFactory,
    ITenantService tenant)
    : BaseRepo<StockMovementEntity>(contextFactory.Create(DatabaseNameConstant.DatabaseName).Database, tenant), IStockMovementRepo
{
    public async Task<ObjectId> CreateAsync(StockMovementEntity entity, CancellationToken cancellationToken = default)
    {
        await Collection.InsertOneAsync(entity, cancellationToken: cancellationToken);
        return entity.Id;
    }

    public async Task<StockMovementEntity?> GetByIdAsync(ObjectId id, CancellationToken cancellationToken = default)
    {
        var filter = Builders<StockMovementEntity>.Filter.Eq(x => x.Id, id);
        return await Collection.Find(TenantFilter() & filter).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<List<StockMovementEntity>> GetByStockIdAsync(ObjectId stockId, CancellationToken cancellationToken = default)
    {
        var filter = Builders<StockMovementEntity>.Filter.Eq(x => x.StockId, stockId);
        return await Collection.Find(TenantFilter() & filter)
            .SortByDescending(x => x.MovementDate)
            .ToListAsync(cancellationToken);
    }

    public async Task<List<StockMovementEntity>> GetByProductIdAsync(ObjectId productId, CancellationToken cancellationToken = default)
    {
        var filter = Builders<StockMovementEntity>.Filter.Eq(x => x.ProductId, productId);
        return await Collection.Find(TenantFilter() & filter)
            .SortByDescending(x => x.MovementDate)
            .ToListAsync(cancellationToken);
    }

    public async Task<List<StockMovementEntity>> GetByWarehouseIdAsync(ObjectId warehouseId, CancellationToken cancellationToken = default)
    {
        var filter = Builders<StockMovementEntity>.Filter.Eq(x => x.WarehouseId, warehouseId);
        return await Collection.Find(TenantFilter() & filter)
            .SortByDescending(x => x.MovementDate)
            .ToListAsync(cancellationToken);
    }

    public async Task<List<StockMovementEntity>> GetByMovementTypeAsync(StockMovementType movementType, CancellationToken cancellationToken = default)
    {
        var filter = Builders<StockMovementEntity>.Filter.Eq(x => x.MovementType, movementType);
        return await Collection.Find(TenantFilter() & filter)
            .SortByDescending(x => x.MovementDate)
            .ToListAsync(cancellationToken);
    }

    public async Task<List<StockMovementEntity>> GetByDateRangeAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default)
    {
        var filter = Builders<StockMovementEntity>.Filter.And(
            Builders<StockMovementEntity>.Filter.Gte(x => x.MovementDate, startDate),
            Builders<StockMovementEntity>.Filter.Lte(x => x.MovementDate, endDate)
        );
        return await Collection.Find(TenantFilter() & filter)
            .SortByDescending(x => x.MovementDate)
            .ToListAsync(cancellationToken);
    }
}
