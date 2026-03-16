using DomnerTech.Backend.Domain.Enums;
using MongoDB.Bson;

namespace DomnerTech.Backend.Domain.Entities;

/// <summary>
/// Represents a stock movement transaction for audit trail and history.
/// </summary>
[MongoCollection("stockMovements")]
public sealed class StockMovementEntity : IBaseEntity, ITenantEntity, IAuditEntity
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
    /// Gets or sets the movement type.
    /// </summary>
    public required StockMovementType MovementType { get; set; }

    /// <summary>
    /// Gets or sets the quantity moved (positive for increase, negative for decrease).
    /// </summary>
    public required decimal Quantity { get; set; }

    /// <summary>
    /// Gets or sets the quantity before this movement.
    /// </summary>
    public required decimal QuantityBefore { get; set; }

    /// <summary>
    /// Gets or sets the quantity after this movement.
    /// </summary>
    public required decimal QuantityAfter { get; set; }

    /// <summary>
    /// Gets or sets the reference document ID (e.g., PurchaseOrderId, SalesOrderId, TransferId).
    /// </summary>
    public ObjectId? ReferenceId { get; set; }

    /// <summary>
    /// Gets or sets the reference document type.
    /// </summary>
    public string? ReferenceType { get; set; }

    /// <summary>
    /// Gets or sets the reference document number.
    /// </summary>
    public string? ReferenceNumber { get; set; }

    /// <summary>
    /// Gets or sets the batch/lot number for this movement.
    /// </summary>
    public string? BatchLotNumber { get; set; }

    /// <summary>
    /// Gets or sets the serial number for this movement.
    /// </summary>
    public string? SerialNumber { get; set; }

    /// <summary>
    /// Gets or sets the unit cost at the time of movement.
    /// </summary>
    public decimal? UnitCost { get; set; }

    /// <summary>
    /// Gets or sets the total cost for this movement.
    /// </summary>
    public decimal? TotalCost { get; set; }

    /// <summary>
    /// Gets or sets notes or remarks for this movement.
    /// </summary>
    public string? Notes { get; set; }

    /// <summary>
    /// Gets or sets the movement date and time.
    /// </summary>
    [Sortable(alias: "movementDate", order: 2)]
    public required DateTime MovementDate { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public ObjectId? UpdatedBy { get; set; }
    public ObjectId? DeletedBy { get; set; }
}
