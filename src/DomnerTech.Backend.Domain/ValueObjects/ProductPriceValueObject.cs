using DomnerTech.Backend.Domain.Enums;
using MongoDB.Bson.Serialization.Attributes;

namespace DomnerTech.Backend.Domain.ValueObjects;

/// <summary>
/// Represents a product price with currency and type.
/// </summary>
[BsonIgnoreExtraElements]
public sealed class ProductPriceValueObject
{
    /// <summary>
    /// Gets or sets the type of price (Retail, Wholesale, Promotion, etc.).
    /// </summary>
    public required PriceType PriceType { get; set; }

    /// <summary>
    /// Gets or sets the currency code.
    /// </summary>
    public required CurrencyCode Currency { get; set; }

    /// <summary>
    /// Gets or sets the price amount.
    /// </summary>
    public required decimal Amount { get; set; }

    /// <summary>
    /// Gets or sets the effective start date for this price.
    /// </summary>
    public DateTime? EffectiveFrom { get; set; }

    /// <summary>
    /// Gets or sets the effective end date for this price.
    /// </summary>
    public DateTime? EffectiveTo { get; set; }

    public ProductPriceValueObject()
    {
    }

    public ProductPriceValueObject(PriceType priceType, CurrencyCode currency, decimal amount)
    {
        PriceType = priceType;
        Currency = currency;
        Amount = amount;
    }

    public override bool Equals(object? obj)
    {
        return obj is ProductPriceValueObject price &&
               PriceType == price.PriceType &&
               Currency == price.Currency &&
               Amount == price.Amount &&
               EffectiveFrom == price.EffectiveFrom &&
               EffectiveTo == price.EffectiveTo;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(PriceType, Currency, Amount, EffectiveFrom, EffectiveTo);
    }

    public override string ToString()
    {
        return $"{Currency} {Amount:N2} ({PriceType})";
    }
}
