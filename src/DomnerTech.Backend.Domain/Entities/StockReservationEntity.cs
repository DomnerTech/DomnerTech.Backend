using MongoDB.Bson;

namespace DomnerTech.Backend.Domain.Entities;

/// <summary>
/// Represents a stock reservation for pending orders.
/// </summary>
[MongoCollection("stockReservations")]
public sealed class StockReservationEntity : IBaseEntity, ITenantEntity, IAuditEntity
{
    [Sortable(alias: "id", order: 1)]
    public ObjectId Id { get; set; }
    public ObjectId CompanyId { get; set; }

    /// <summary>
    /// Gets or sets the stock ID.
    /// </summary>
    public required ObjectId StockId { get; set; }

    /// <summary>
    /// Gets or sets the product ID.
    /// </summary>
    public required ObjectId ProductId { get; set; }

    /// <summary>
    /// Gets or sets the warehouse ID.
    /// </summary>
    public required ObjectId WarehouseId { get; set; }

    /// <summary>
    /// Gets or sets the reserved quantity.
    /// </summary>
    public required decimal Quantity { get; set; }

    /// <summary>
    /// Gets or sets the order ID that reserved this stock.
    /// </summary>
    public required ObjectId OrderId { get; set; }

    /// <summary>
    /// Gets or sets the order line item ID.
    /// </summary>
    public ObjectId? OrderLineId { get; set; }

    /// <summary>
    /// Gets or sets the reservation expiry date.
    /// </summary>
    public DateTime? ExpiresAt { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether this reservation is fulfilled.
    /// </summary>
    public bool IsFulfilled { get; set; }

    /// <summary>
    /// Gets or sets the fulfillment date.
    /// </summary>
    public DateTime? FulfilledAt { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether this reservation is released.
    /// </summary>
    public bool IsReleased { get; set; }

    /// <summary>
    /// Gets or sets the release date.
    /// </summary>
    public DateTime? ReleasedAt { get; set; }

    /// <summary>
    /// Gets or sets notes for this reservation.
    /// </summary>
    public string? Notes { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public ObjectId? UpdatedBy { get; set; }
    public ObjectId? DeletedBy { get; set; }
}
