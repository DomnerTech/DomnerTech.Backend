using DomnerTech.Backend.Domain.Entities;
using MongoDB.Bson;

namespace DomnerTech.Backend.Application.IRepo;

/// <summary>
/// Repository interface for Stock entity operations.
/// </summary>
public interface IStockRepo : IBaseRepo
{
    /// <summary>
    /// Creates a new stock record.
    /// </summary>
    Task<ObjectId> CreateAsync(StockEntity entity, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing stock record.
    /// </summary>
    Task UpdateAsync(StockEntity entity, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a stock record by ID.
    /// </summary>
    Task<StockEntity?> GetByIdAsync(ObjectId id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets stock for a product in a specific warehouse.
    /// </summary>
    Task<StockEntity?> GetByProductAndWarehouseAsync(ObjectId productId, ObjectId warehouseId, ObjectId? variantId = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets all stock records for a product across all warehouses.
    /// </summary>
    Task<List<StockEntity>> GetByProductIdAsync(ObjectId productId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets all stock records for a warehouse.
    /// </summary>
    Task<List<StockEntity>> GetByWarehouseIdAsync(ObjectId warehouseId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets low stock items (available quantity <= reorder level).
    /// </summary>
    Task<List<StockEntity>> GetLowStockItemsAsync(ObjectId? warehouseId = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets out of stock items (available quantity = 0).
    /// </summary>
    Task<List<StockEntity>> GetOutOfStockItemsAsync(ObjectId? warehouseId = null, CancellationToken cancellationToken = default);
}
