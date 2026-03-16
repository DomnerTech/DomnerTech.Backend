using MongoDB.Bson;

namespace DomnerTech.Backend.Domain.Events;

/// <summary>
/// Domain event raised when stock falls below reorder level.
/// </summary>
public sealed record LowStockAlertEvent
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
    /// Gets the product SKU.
    /// </summary>
    public required string ProductSku { get; init; }

    /// <summary>
    /// Gets the product name.
    /// </summary>
    public required string ProductName { get; init; }

    /// <summary>
    /// Gets the warehouse ID.
    /// </summary>
    public required ObjectId WarehouseId { get; init; }

    /// <summary>
    /// Gets the warehouse name.
    /// </summary>
    public required string WarehouseName { get; init; }

    /// <summary>
    /// Gets the company/tenant ID.
    /// </summary>
    public required ObjectId CompanyId { get; init; }

    /// <summary>
    /// Gets the current available quantity.
    /// </summary>
    public required decimal AvailableQuantity { get; init; }

    /// <summary>
    /// Gets the reorder level.
    /// </summary>
    public required decimal ReorderLevel { get; init; }

    /// <summary>
    /// Gets the recommended reorder quantity.
    /// </summary>
    public decimal? ReorderQuantity { get; init; }

    /// <summary>
    /// Gets the event timestamp.
    /// </summary>
    public DateTime OccurredAt { get; init; } = DateTime.UtcNow;

    /// <summary>
    /// Gets the alert severity level.
    /// </summary>
    public string Severity { get; init; } = "Warning";
}
