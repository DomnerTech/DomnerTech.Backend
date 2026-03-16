using MongoDB.Bson;

namespace DomnerTech.Backend.Domain.Events;

/// <summary>
/// Domain event raised when stock is reserved for an order.
/// </summary>
public sealed record StockReservedEvent
{
    /// <summary>
    /// Gets the reservation ID.
    /// </summary>
    public required ObjectId ReservationId { get; init; }

    /// <summary>
    /// Gets the stock ID.
    /// </summary>
    public required ObjectId StockId { get; init; }

    /// <summary>
    /// Gets the product ID.
    /// </summary>
    public required ObjectId ProductId { get; init; }

    /// <summary>
    /// Gets the warehouse ID.
    /// </summary>
    public required ObjectId WarehouseId { get; init; }

    /// <summary>
    /// Gets the company/tenant ID.
    /// </summary>
    public required ObjectId CompanyId { get; init; }

    /// <summary>
    /// Gets the order ID.
    /// </summary>
    public required ObjectId OrderId { get; init; }

    /// <summary>
    /// Gets the reserved quantity.
    /// </summary>
    public required decimal Quantity { get; init; }

    /// <summary>
    /// Gets the reservation expiry date.
    /// </summary>
    public DateTime? ExpiresAt { get; init; }

    /// <summary>
    /// Gets the event timestamp.
    /// </summary>
    public DateTime OccurredAt { get; init; } = DateTime.UtcNow;

    /// <summary>
    /// Gets the user ID who created the reservation.
    /// </summary>
    public ObjectId? ReservedBy { get; init; }
}
