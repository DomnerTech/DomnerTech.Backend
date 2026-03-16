using DomnerTech.Backend.Domain.Enums;
using MongoDB.Bson;

namespace DomnerTech.Backend.Domain.Entities;

/// <summary>
/// Represents a manual inventory adjustment.
/// </summary>
[MongoCollection("inventoryAdjustments")]
public sealed class InventoryAdjustmentEntity : IBaseEntity, ITenantEntity, IAuditEntity
{
    [Sortable(alias: "id", order: 1)]
    public ObjectId Id { get; set; }
    public ObjectId CompanyId { get; set; }

    /// <summary>
    /// Gets or sets the adjustment number.
    /// </summary>
    [Sortable(alias: "adjustmentNumber", order: 2)]
    public required string AdjustmentNumber { get; set; }

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
    /// Gets or sets the adjustment reason.
    /// </summary>
    public required InventoryAdjustmentReason Reason { get; set; }

    /// <summary>
    /// Gets or sets the quantity before adjustment.
    /// </summary>
    public required decimal QuantityBefore { get; set; }

    /// <summary>
    /// Gets or sets the quantity after adjustment.
    /// </summary>
    public required decimal QuantityAfter { get; set; }

    /// <summary>
    /// Gets or sets the adjustment quantity (positive for increase, negative for decrease).
    /// </summary>
    public required decimal AdjustmentQuantity { get; set; }

    /// <summary>
    /// Gets or sets notes explaining the adjustment.
    /// </summary>
    public string? Notes { get; set; }

    /// <summary>
    /// Gets or sets the adjustment date.
    /// </summary>
    [Sortable(alias: "adjustmentDate", order: 3)]
    public required DateTime AdjustmentDate { get; set; }

    /// <summary>
    /// Gets or sets the approved by user ID.
    /// </summary>
    public ObjectId? ApprovedBy { get; set; }

    /// <summary>
    /// Gets or sets the approval date.
    /// </summary>
    public DateTime? ApprovedAt { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public ObjectId? UpdatedBy { get; set; }
    public ObjectId? DeletedBy { get; set; }
}
