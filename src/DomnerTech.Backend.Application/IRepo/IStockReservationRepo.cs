using DomnerTech.Backend.Domain.Entities;
using MongoDB.Bson;

namespace DomnerTech.Backend.Application.IRepo;

/// <summary>
/// Repository interface for StockReservation entity operations.
/// </summary>
public interface IStockReservationRepo : IBaseRepo
{
    /// <summary>
    /// Creates a new stock reservation.
    /// </summary>
    Task<ObjectId> CreateAsync(StockReservationEntity entity, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing stock reservation.
    /// </summary>
    Task UpdateAsync(StockReservationEntity entity, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a stock reservation by ID.
    /// </summary>
    Task<StockReservationEntity?> GetByIdAsync(ObjectId id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets all reservations for a stock record.
    /// </summary>
    Task<List<StockReservationEntity>> GetByStockIdAsync(ObjectId stockId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets all reservations for an order.
    /// </summary>
    Task<List<StockReservationEntity>> GetByOrderIdAsync(ObjectId orderId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets active (unfulfilled and unreleased) reservations for a product.
    /// </summary>
    Task<List<StockReservationEntity>> GetActiveReservationsAsync(ObjectId productId, ObjectId warehouseId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets expired reservations that need to be released.
    /// </summary>
    Task<List<StockReservationEntity>> GetExpiredReservationsAsync(CancellationToken cancellationToken = default);
}
