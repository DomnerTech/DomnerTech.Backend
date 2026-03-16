using DomnerTech.Backend.Domain.Enums;
using MongoDB.Bson;

namespace DomnerTech.Backend.Domain.Entities;

/// <summary>
/// Represents a stock counting/physical inventory process.
/// </summary>
[MongoCollection("stockCounts")]
public sealed class StockCountEntity : IBaseEntity, ITenantEntity, IAuditEntity
{
    [Sortable(alias: "id", order: 1)]
    public ObjectId Id { get; set; }
    public ObjectId CompanyId { get; set; }

    /// <summary>
    /// Gets or sets the stock count number.
    /// </summary>
    [Sortable(alias: "countNumber", order: 2)]
    public required string CountNumber { get; set; }

    /// <summary>
    /// Gets or sets the warehouse ID.
    /// </summary>
    public required ObjectId WarehouseId { get; set; }

    /// <summary>
    /// Gets or sets the count status.
    /// </summary>
    [Sortable(alias: "status", order: 3)]
    public StockCountStatus Status { get; set; } = StockCountStatus.Planned;

    /// <summary>
    /// Gets or sets the planned count date.
    /// </summary>
    public required DateTime PlannedDate { get; set; }

    /// <summary>
    /// Gets or sets the actual start date.
    /// </summary>
    public DateTime? StartedAt { get; set; }

    /// <summary>
    /// Gets or sets the completion date.
    /// </summary>
    public DateTime? CompletedAt { get; set; }

    /// <summary>
    /// Gets or sets the user ID who started the count.
    /// </summary>
    public ObjectId? StartedBy { get; set; }

    /// <summary>
    /// Gets or sets the user ID who completed the count.
    /// </summary>
    public ObjectId? CompletedBy { get; set; }

    /// <summary>
    /// Gets or sets the user ID who approved the count.
    /// </summary>
    public ObjectId? ApprovedBy { get; set; }

    /// <summary>
    /// Gets or sets the approval date.
    /// </summary>
    public DateTime? ApprovedAt { get; set; }

    /// <summary>
    /// Gets or sets the stock count items.
    /// </summary>
    public List<StockCountItemValueObject>? Items { get; set; }

    /// <summary>
    /// Gets or sets notes for this stock count.
    /// </summary>
    public string? Notes { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public ObjectId? UpdatedBy { get; set; }
    public ObjectId? DeletedBy { get; set; }
}

/// <summary>
/// Represents an individual item in a stock count.
/// </summary>
public sealed class StockCountItemValueObject
{
    /// <summary>
    /// Gets or sets the stock ID.
    /// </summary>
    public required ObjectId StockId { get; set; }

    /// <summary>
    /// Gets or sets the product ID.
    /// </summary>
    public required ObjectId ProductId { get; set; }

    /// <summary>
    /// Gets or sets the expected quantity from system.
    /// </summary>
    public required decimal ExpectedQuantity { get; set; }

    /// <summary>
    /// Gets or sets the actual counted quantity.
    /// </summary>
    public decimal? CountedQuantity { get; set; }

    /// <summary>
    /// Gets or sets the variance (CountedQuantity - ExpectedQuantity).
    /// </summary>
    public decimal Variance => (CountedQuantity ?? 0) - ExpectedQuantity;

    /// <summary>
    /// Gets or sets notes for this item.
    /// </summary>
    public string? Notes { get; set; }
}
