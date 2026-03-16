using MongoDB.Bson;

namespace DomnerTech.Backend.Domain.Events;

/// <summary>
/// Domain event raised when a new product is created.
/// </summary>
public sealed record ProductCreatedEvent
{
    /// <summary>
    /// Gets the product ID.
    /// </summary>
    public required ObjectId ProductId { get; init; }

    /// <summary>
    /// Gets the company/tenant ID.
    /// </summary>
    public required ObjectId CompanyId { get; init; }

    /// <summary>
    /// Gets the product SKU.
    /// </summary>
    public required string Sku { get; init; }

    /// <summary>
    /// Gets the product name (English).
    /// </summary>
    public required string Name { get; init; }

    /// <summary>
    /// Gets the category ID.
    /// </summary>
    public required ObjectId CategoryId { get; init; }

    /// <summary>
    /// Gets the event timestamp.
    /// </summary>
    public DateTime OccurredAt { get; init; } = DateTime.UtcNow;

    /// <summary>
    /// Gets the user ID who created the product.
    /// </summary>
    public ObjectId? CreatedBy { get; init; }
}
