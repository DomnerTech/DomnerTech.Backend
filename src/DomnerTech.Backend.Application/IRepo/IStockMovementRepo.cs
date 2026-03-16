using DomnerTech.Backend.Domain.Entities;
using DomnerTech.Backend.Domain.Enums;
using MongoDB.Bson;

namespace DomnerTech.Backend.Application.IRepo;

/// <summary>
/// Repository interface for StockMovement entity operations.
/// </summary>
public interface IStockMovementRepo : IBaseRepo
{
    /// <summary>
    /// Creates a new stock movement record.
    /// </summary>
    Task<ObjectId> CreateAsync(StockMovementEntity entity, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a stock movement by ID.
    /// </summary>
    Task<StockMovementEntity?> GetByIdAsync(ObjectId id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets all movements for a stock record.
    /// </summary>
    Task<List<StockMovementEntity>> GetByStockIdAsync(ObjectId stockId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets all movements for a product.
    /// </summary>
    Task<List<StockMovementEntity>> GetByProductIdAsync(ObjectId productId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets all movements for a warehouse.
    /// </summary>
    Task<List<StockMovementEntity>> GetByWarehouseIdAsync(ObjectId warehouseId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets movements by type.
    /// </summary>
    Task<List<StockMovementEntity>> GetByMovementTypeAsync(StockMovementType movementType, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets movements within a date range.
    /// </summary>
    Task<List<StockMovementEntity>> GetByDateRangeAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default);
}
