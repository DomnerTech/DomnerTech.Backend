using DomnerTech.Backend.Domain.ValueObjects;
using MongoDB.Bson;

namespace DomnerTech.Backend.Domain.Entities;

/// <summary>
/// Represents stock inventory for a product in a specific warehouse (Aggregate Root).
/// </summary>
[MongoCollection("stocks")]
public sealed class StockEntity : IBaseEntity, ITenantEntity, IAuditEntity
{
    [Sortable(alias: "id", order: 1)]
    public ObjectId Id { get; set; }
    public ObjectId CompanyId { get; set; }

    /// <summary>
    /// Gets or sets the product ID.
    /// </summary>
    public required ObjectId ProductId { get; set; }

    /// <summary>
    /// Gets or sets the product variant ID (if applicable).
    /// </summary>
    public ObjectId? VariantId { get; set; }

    /// <summary>
    /// Gets or sets the warehouse ID.
    /// </summary>
    public required ObjectId WarehouseId { get; set; }

    /// <summary>
    /// Gets or sets the stock level information.
    /// </summary>
    public required StockLevelValueObject StockLevel { get; set; }

    /// <summary>
    /// Gets or sets the batch/lot tracking information.
    /// </summary>
    public List<BatchLotValueObject>? Batches { get; set; }

    /// <summary>
    /// Gets or sets the serial numbers for tracked items.
    /// </summary>
    public List<SerialNumberValueObject>? SerialNumbers { get; set; }

    /// <summary>
    /// Gets or sets the last stock count date.
    /// </summary>
    public DateTime? LastCountedAt { get; set; }

    /// <summary>
    /// Gets or sets the last stock movement date.
    /// </summary>
    [Sortable(alias: "lastMovementAt", order: 2)]
    public DateTime? LastMovementAt { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public ObjectId? UpdatedBy { get; set; }
    public ObjectId? DeletedBy { get; set; }
}
