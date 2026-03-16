using DomnerTech.Backend.Application.Constants;
using DomnerTech.Backend.Application.IRepo;
using DomnerTech.Backend.Application.Services;
using DomnerTech.Backend.Domain.Entities;
using DomnerTech.Backend.Infrastructure.MongoDb;
using MongoDB.Bson;
using MongoDB.Driver;

namespace DomnerTech.Backend.Infrastructure.Repo;

/// <summary>
/// Repository implementation for StockReservation entity.
/// </summary>
public sealed class StockReservationRepo(
    IMongoDbContextFactory contextFactory,
    ITenantService tenant)
    : BaseRepo<StockReservationEntity>(contextFactory.Create(DatabaseNameConstant.DatabaseName).Database, tenant), IStockReservationRepo
{
    public async Task<ObjectId> CreateAsync(StockReservationEntity entity, CancellationToken cancellationToken = default)
    {
        await Collection.InsertOneAsync(entity, cancellationToken: cancellationToken);
        return entity.Id;
    }

    public async Task UpdateAsync(StockReservationEntity entity, CancellationToken cancellationToken = default)
    {
        var filter = Builders<StockReservationEntity>.Filter.Eq(x => x.Id, entity.Id);
        await Collection.ReplaceOneAsync(TenantFilter() & filter, entity, cancellationToken: cancellationToken);
    }

    public async Task<StockReservationEntity?> GetByIdAsync(ObjectId id, CancellationToken cancellationToken = default)
    {
        var filter = Builders<StockReservationEntity>.Filter.Eq(x => x.Id, id);
        return await Collection.Find(TenantFilter() & filter).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<List<StockReservationEntity>> GetByStockIdAsync(ObjectId stockId, CancellationToken cancellationToken = default)
    {
        var filter = Builders<StockReservationEntity>.Filter.Eq(x => x.StockId, stockId);
        return await Collection.Find(TenantFilter() & filter)
            .SortByDescending(x => x.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<List<StockReservationEntity>> GetByOrderIdAsync(ObjectId orderId, CancellationToken cancellationToken = default)
    {
        var filter = Builders<StockReservationEntity>.Filter.Eq(x => x.OrderId, orderId);
        return await Collection.Find(TenantFilter() & filter).ToListAsync(cancellationToken);
    }

    public async Task<List<StockReservationEntity>> GetActiveReservationsAsync(ObjectId productId, ObjectId warehouseId, CancellationToken cancellationToken = default)
    {
        var filter = Builders<StockReservationEntity>.Filter.And(
            Builders<StockReservationEntity>.Filter.Eq(x => x.ProductId, productId),
            Builders<StockReservationEntity>.Filter.Eq(x => x.WarehouseId, warehouseId),
            Builders<StockReservationEntity>.Filter.Eq(x => x.IsFulfilled, false),
            Builders<StockReservationEntity>.Filter.Eq(x => x.IsReleased, false)
        );
        return await Collection.Find(TenantFilter() & filter).ToListAsync(cancellationToken);
    }

    public async Task<List<StockReservationEntity>> GetExpiredReservationsAsync(CancellationToken cancellationToken = default)
    {
        var now = DateTime.UtcNow;
        var filter = Builders<StockReservationEntity>.Filter.And(
            Builders<StockReservationEntity>.Filter.Lte(x => x.ExpiresAt, now),
            Builders<StockReservationEntity>.Filter.Eq(x => x.IsFulfilled, false),
            Builders<StockReservationEntity>.Filter.Eq(x => x.IsReleased, false),
            Builders<StockReservationEntity>.Filter.Ne(x => x.ExpiresAt, null)
        );
        return await Collection.Find(TenantFilter() & filter).ToListAsync(cancellationToken);
    }
}
