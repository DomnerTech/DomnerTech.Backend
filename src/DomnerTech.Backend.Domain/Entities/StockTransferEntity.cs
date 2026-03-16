using DomnerTech.Backend.Domain.Enums;
using MongoDB.Bson;

namespace DomnerTech.Backend.Domain.Entities;

/// <summary>
/// Represents a stock transfer between warehouses (Aggregate Root).
/// </summary>
[MongoCollection("stockTransfers")]
public sealed class StockTransferEntity : IBaseEntity, ITenantEntity, IAuditEntity
{
    [Sortable(alias: "id", order: 1)]
    public ObjectId Id { get; set; }
    public ObjectId CompanyId { get; set; }

    /// <summary>
    /// Gets or sets the transfer number.
    /// </summary>
    [Sortable(alias: "transferNumber", order: 2)]
    public required string TransferNumber { get; set; }

    /// <summary>
    /// Gets or sets the source warehouse ID.
    /// </summary>
    public required ObjectId FromWarehouseId { get; set; }

    /// <summary>
    /// Gets or sets the destination warehouse ID.
    /// </summary>
    public required ObjectId ToWarehouseId { get; set; }

    /// <summary>
    /// Gets or sets the product ID.
    /// </summary>
    public required ObjectId ProductId { get; set; }

    /// <summary>
    /// Gets or sets the product variant ID (if applicable).
    /// </summary>
    public ObjectId? VariantId { get; set; }

    /// <summary>
    /// Gets or sets the quantity to transfer.
    /// </summary>
    public required decimal Quantity { get; set; }

    /// <summary>
    /// Gets or sets the transfer status.
    /// </summary>
    [Sortable(alias: "status", order: 3)]
    public StockTransferStatus Status { get; set; } = StockTransferStatus.Pending;

    /// <summary>
    /// Gets or sets the requested by user ID.
    /// </summary>
    public required ObjectId RequestedBy { get; set; }

    /// <summary>
    /// Gets or sets the request date.
    /// </summary>
    public required DateTime RequestedAt { get; set; }

    /// <summary>
    /// Gets or sets the approved by user ID.
    /// </summary>
    public ObjectId? ApprovedBy { get; set; }

    /// <summary>
    /// Gets or sets the approval date.
    /// </summary>
    public DateTime? ApprovedAt { get; set; }

    /// <summary>
    /// Gets or sets the shipped by user ID.
    /// </summary>
    public ObjectId? ShippedBy { get; set; }

    /// <summary>
    /// Gets or sets the shipment date.
    /// </summary>
    public DateTime? ShippedAt { get; set; }

    /// <summary>
    /// Gets or sets the received by user ID.
    /// </summary>
    public ObjectId? ReceivedBy { get; set; }

    /// <summary>
    /// Gets or sets the received date.
    /// </summary>
    public DateTime? ReceivedAt { get; set; }

    /// <summary>
    /// Gets or sets the quantity actually received (may differ from requested).
    /// </summary>
    public decimal? QuantityReceived { get; set; }

    /// <summary>
    /// Gets or sets notes or remarks for this transfer.
    /// </summary>
    public string? Notes { get; set; }

    /// <summary>
    /// Gets or sets the rejection reason if status is Rejected.
    /// </summary>
    public string? RejectionReason { get; set; }

    /// <summary>
    /// Gets or sets the cancellation reason if status is Cancelled.
    /// </summary>
    public string? CancellationReason { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public ObjectId? UpdatedBy { get; set; }
    public ObjectId? DeletedBy { get; set; }
}
