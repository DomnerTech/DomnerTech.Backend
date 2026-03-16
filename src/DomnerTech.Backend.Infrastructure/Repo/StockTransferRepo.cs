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
/// Repository implementation for StockTransfer entity.
/// </summary>
public sealed class StockTransferRepo(
    IMongoDbContextFactory contextFactory,
    ITenantService tenant)
    : BaseRepo<StockTransferEntity>(contextFactory.Create(DatabaseNameConstant.DatabaseName).Database, tenant), IStockTransferRepo
{
    public async Task<ObjectId> CreateAsync(StockTransferEntity entity, CancellationToken cancellationToken = default)
    {
        await Collection.InsertOneAsync(entity, cancellationToken: cancellationToken);
        return entity.Id;
    }

    public async Task UpdateAsync(StockTransferEntity entity, CancellationToken cancellationToken = default)
    {
        var filter = Builders<StockTransferEntity>.Filter.Eq(x => x.Id, entity.Id);
        await Collection.ReplaceOneAsync(TenantFilter() & filter, entity, cancellationToken: cancellationToken);
    }

    public async Task<StockTransferEntity?> GetByIdAsync(ObjectId id, CancellationToken cancellationToken = default)
    {
        var filter = Builders<StockTransferEntity>.Filter.Eq(x => x.Id, id);
        return await Collection.Find(TenantFilter() & filter).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<StockTransferEntity?> GetByTransferNumberAsync(string transferNumber, CancellationToken cancellationToken = default)
    {
        var filter = Builders<StockTransferEntity>.Filter.Eq(x => x.TransferNumber, transferNumber);
        return await Collection.Find(TenantFilter() & filter).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<List<StockTransferEntity>> GetByStatusAsync(StockTransferStatus status, CancellationToken cancellationToken = default)
    {
        var filter = Builders<StockTransferEntity>.Filter.Eq(x => x.Status, status);
        return await Collection.Find(TenantFilter() & filter)
            .SortByDescending(x => x.RequestedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<List<StockTransferEntity>> GetByWarehouseAsync(ObjectId warehouseId, CancellationToken cancellationToken = default)
    {
        var filter = Builders<StockTransferEntity>.Filter.Or(
            Builders<StockTransferEntity>.Filter.Eq(x => x.FromWarehouseId, warehouseId),
            Builders<StockTransferEntity>.Filter.Eq(x => x.ToWarehouseId, warehouseId)
        );
        return await Collection.Find(TenantFilter() & filter)
            .SortByDescending(x => x.RequestedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<string> GetNextTransferNumberAsync(CancellationToken cancellationToken = default)
    {
        var count = await Collection.CountDocumentsAsync(TenantFilter(), cancellationToken: cancellationToken);
        var date = DateTime.UtcNow.ToString("yyyyMMdd");
        return $"TRF-{date}-{(count + 1):D6}";
    }
}
