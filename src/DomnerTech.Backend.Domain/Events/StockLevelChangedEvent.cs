using DomnerTech.Backend.Domain.Enums;
using MongoDB.Bson;

namespace DomnerTech.Backend.Domain.Events;

/// <summary>
/// Domain event raised when stock level changes.
/// </summary>
public sealed record StockLevelChangedEvent
{
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
    /// Gets the movement type.
    /// </summary>
    public required StockMovementType MovementType { get; init; }

    /// <summary>
    /// Gets the quantity before change.
    /// </summary>
    public required decimal QuantityBefore { get; init; }

    /// <summary>
    /// Gets the quantity after change.
    /// </summary>
    public required decimal QuantityAfter { get; init; }

    /// <summary>
    /// Gets the quantity changed.
    /// </summary>
    public required decimal QuantityChanged { get; init; }

    /// <summary>
    /// Gets the event timestamp.
    /// </summary>
    public DateTime OccurredAt { get; init; } = DateTime.UtcNow;

    /// <summary>
    /// Gets the user ID who made the change.
    /// </summary>
    public ObjectId? ChangedBy { get; init; }
}
