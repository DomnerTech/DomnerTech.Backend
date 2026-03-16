using DomnerTech.Backend.Domain.Enums;
using MongoDB.Bson;

namespace DomnerTech.Backend.Domain.Events;

/// <summary>
/// Domain event raised when a stock transfer is completed.
/// </summary>
public sealed record StockTransferCompletedEvent
{
    /// <summary>
    /// Gets the transfer ID.
    /// </summary>
    public required ObjectId TransferId { get; init; }

    /// <summary>
    /// Gets the transfer number.
    /// </summary>
    public required string TransferNumber { get; init; }

    /// <summary>
    /// Gets the product ID.
    /// </summary>
    public required ObjectId ProductId { get; init; }

    /// <summary>
    /// Gets the source warehouse ID.
    /// </summary>
    public required ObjectId FromWarehouseId { get; init; }

    /// <summary>
    /// Gets the destination warehouse ID.
    /// </summary>
    public required ObjectId ToWarehouseId { get; init; }

    /// <summary>
    /// Gets the company/tenant ID.
    /// </summary>
    public required ObjectId CompanyId { get; init; }

    /// <summary>
    /// Gets the quantity transferred.
    /// </summary>
    public required decimal Quantity { get; init; }

    /// <summary>
    /// Gets the quantity actually received.
    /// </summary>
    public decimal? QuantityReceived { get; init; }

    /// <summary>
    /// Gets the transfer status.
    /// </summary>
    public required StockTransferStatus Status { get; init; }

    /// <summary>
    /// Gets the event timestamp.
    /// </summary>
    public DateTime OccurredAt { get; init; } = DateTime.UtcNow;

    /// <summary>
    /// Gets the user ID who completed the transfer.
    /// </summary>
    public ObjectId? CompletedBy { get; init; }
}
