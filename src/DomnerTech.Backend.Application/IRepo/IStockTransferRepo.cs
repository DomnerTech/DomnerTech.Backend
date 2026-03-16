using DomnerTech.Backend.Domain.Entities;
using DomnerTech.Backend.Domain.Enums;
using MongoDB.Bson;

namespace DomnerTech.Backend.Application.IRepo;

/// <summary>
/// Repository interface for StockTransfer entity operations.
/// </summary>
public interface IStockTransferRepo : IBaseRepo
{
    /// <summary>
    /// Creates a new stock transfer.
    /// </summary>
    Task<ObjectId> CreateAsync(StockTransferEntity entity, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing stock transfer.
    /// </summary>
    Task UpdateAsync(StockTransferEntity entity, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a stock transfer by ID.
    /// </summary>
    Task<StockTransferEntity?> GetByIdAsync(ObjectId id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a stock transfer by transfer number.
    /// </summary>
    Task<StockTransferEntity?> GetByTransferNumberAsync(string transferNumber, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets stock transfers by status.
    /// </summary>
    Task<List<StockTransferEntity>> GetByStatusAsync(StockTransferStatus status, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets stock transfers for a warehouse (from or to).
    /// </summary>
    Task<List<StockTransferEntity>> GetByWarehouseAsync(ObjectId warehouseId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets the next transfer number.
    /// </summary>
    Task<string> GetNextTransferNumberAsync(CancellationToken cancellationToken = default);
}
