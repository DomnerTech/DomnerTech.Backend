using DomnerTech.Backend.Domain.Enums;
using MongoDB.Bson;

namespace DomnerTech.Backend.Domain.Events;

/// <summary>
/// Domain event raised when product price changes.
/// </summary>
public sealed record ProductPriceChangedEvent
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
    /// Gets the price type.
    /// </summary>
    public required PriceType PriceType { get; init; }

    /// <summary>
    /// Gets the currency.
    /// </summary>
    public required CurrencyCode Currency { get; init; }

    /// <summary>
    /// Gets the old price.
    /// </summary>
    public required decimal OldPrice { get; init; }

    /// <summary>
    /// Gets the new price.
    /// </summary>
    public required decimal NewPrice { get; init; }

    /// <summary>
    /// Gets the event timestamp.
    /// </summary>
    public DateTime OccurredAt { get; init; } = DateTime.UtcNow;

    /// <summary>
    /// Gets the user ID who changed the price.
    /// </summary>
    public ObjectId? ChangedBy { get; init; }
}
