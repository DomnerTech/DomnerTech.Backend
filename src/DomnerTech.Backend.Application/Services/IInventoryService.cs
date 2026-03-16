
using DomnerTech.Backend.Domain.Enums;
using MongoDB.Bson;

namespace DomnerTech.Backend.Application.Services;

/// <summary>
/// Service interface for inventory management operations.
/// </summary>
public interface IInventoryService : IBaseService
{
    /// <summary>
    /// Adjusts stock quantity and records movement.
    /// </summary>
    Task<bool> AdjustStockAsync(
        ObjectId productId,
        ObjectId warehouseId,
        decimal quantity,
        StockMovementType movementType,
        ObjectId? variantId = null,
        string? batchLotNumber = null,
        string? serialNumber = null,
        ObjectId? referenceId = null,
        string? notes = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Reserves stock for an order.
    /// </summary>
    Task<ObjectId> ReserveStockAsync(
        ObjectId productId,
        ObjectId warehouseId,
        decimal quantity,
        ObjectId orderId,
        DateTime? expiresAt = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Releases a stock reservation.
    /// </summary>
    Task<bool> ReleaseReservationAsync(ObjectId reservationId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Fulfills a stock reservation (converts to sale).
    /// </summary>
    Task<bool> FulfillReservationAsync(ObjectId reservationId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets available stock quantity (on hand - reserved).
    /// </summary>
    Task<decimal> GetAvailableQuantityAsync(ObjectId productId, ObjectId warehouseId, ObjectId? variantId = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Checks if sufficient stock is available.
    /// </summary>
    Task<bool> IsStockAvailableAsync(ObjectId productId, ObjectId warehouseId, decimal requiredQuantity, ObjectId? variantId = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Processes expired stock reservations.
    /// </summary>
    Task ProcessExpiredReservationsAsync(CancellationToken cancellationToken = default);
}
