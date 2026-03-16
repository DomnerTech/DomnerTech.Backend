using DomnerTech.Backend.Application.Constants;
using DomnerTech.Backend.Application.IRepo;
using DomnerTech.Backend.Application.Services;
using DomnerTech.Backend.Domain.Entities;
using DomnerTech.Backend.Infrastructure.MongoDb;
using MongoDB.Bson;
using MongoDB.Driver;

namespace DomnerTech.Backend.Infrastructure.Repo;

/// <summary>
/// Repository implementation for Stock entity.
/// </summary>
public sealed class StockRepo(
    IMongoDbContextFactory contextFactory,
    ITenantService tenant)
    : BaseRepo<StockEntity>(contextFactory.Create(DatabaseNameConstant.DatabaseName).Database, tenant), IStockRepo
{
    public async Task<ObjectId> CreateAsync(StockEntity entity, CancellationToken cancellationToken = default)
    {
        await Collection.InsertOneAsync(entity, cancellationToken: cancellationToken);
        return entity.Id;
    }

    public async Task UpdateAsync(StockEntity entity, CancellationToken cancellationToken = default)
    {
        var filter = Builders<StockEntity>.Filter.Eq(x => x.Id, entity.Id);
        await Collection.ReplaceOneAsync(TenantFilter() & filter, entity, cancellationToken: cancellationToken);
    }

    public async Task<StockEntity?> GetByIdAsync(ObjectId id, CancellationToken cancellationToken = default)
    {
        var filter = Builders<StockEntity>.Filter.Eq(x => x.Id, id);
        return await Collection.Find(TenantFilter() & filter).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<StockEntity?> GetByProductAndWarehouseAsync(ObjectId productId, ObjectId warehouseId, ObjectId? variantId = null, CancellationToken cancellationToken = default)
    {
        var filterBuilder = Builders<StockEntity>.Filter;
        var filter = filterBuilder.And(
            filterBuilder.Eq(x => x.ProductId, productId),
            filterBuilder.Eq(x => x.WarehouseId, warehouseId)
        );

        if (variantId.HasValue)
        {
            filter &= filterBuilder.Eq(x => x.VariantId, variantId.Value);
        }
        else
        {
            filter &= filterBuilder.Or(
                filterBuilder.Eq(x => x.VariantId, null),
                filterBuilder.Exists(x => x.VariantId, false)
            );
        }

        return await Collection.Find(TenantFilter() & filter).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<List<StockEntity>> GetByProductIdAsync(ObjectId productId, CancellationToken cancellationToken = default)
    {
        var filter = Builders<StockEntity>.Filter.Eq(x => x.ProductId, productId);
        return await Collection.Find(TenantFilter() & filter).ToListAsync(cancellationToken);
    }

    public async Task<List<StockEntity>> GetByWarehouseIdAsync(ObjectId warehouseId, CancellationToken cancellationToken = default)
    {
        var filter = Builders<StockEntity>.Filter.Eq(x => x.WarehouseId, warehouseId);
        return await Collection.Find(TenantFilter() & filter).ToListAsync(cancellationToken);
    }

    public async Task<List<StockEntity>> GetLowStockItemsAsync(ObjectId? warehouseId = null, CancellationToken cancellationToken = default)
    {
        var filterBuilder = Builders<StockEntity>.Filter;
        var filter = filterBuilder.Lte("StockLevel.AvailableQuantity", "StockLevel.ReorderLevel");

        if (warehouseId.HasValue)
        {
            filter &= filterBuilder.Eq(x => x.WarehouseId, warehouseId.Value);
        }

        return await Collection.Find(TenantFilter() & filter)
            .SortBy(x => x.StockLevel.AvailableQuantity)
            .ToListAsync(cancellationToken);
    }

    public async Task<List<StockEntity>> GetOutOfStockItemsAsync(ObjectId? warehouseId = null, CancellationToken cancellationToken = default)
    {
        var filterBuilder = Builders<StockEntity>.Filter;
        var filter = filterBuilder.Lte("StockLevel.AvailableQuantity", 0);

        if (warehouseId.HasValue)
        {
            filter &= filterBuilder.Eq(x => x.WarehouseId, warehouseId.Value);
        }

        return await Collection.Find(TenantFilter() & filter).ToListAsync(cancellationToken);
    }
}
