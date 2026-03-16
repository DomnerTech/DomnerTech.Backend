using MongoDB.Bson.Serialization.Attributes;

namespace DomnerTech.Backend.Domain.ValueObjects;

/// <summary>
/// Represents stock level information with reorder points.
/// </summary>
[BsonIgnoreExtraElements]
public sealed class StockLevelValueObject
{
    /// <summary>
    /// Gets or sets the current quantity on hand.
    /// </summary>
    public decimal QuantityOnHand { get; set; }

    /// <summary>
    /// Gets or sets the reserved quantity (for pending orders).
    /// </summary>
    public decimal ReservedQuantity { get; set; }

    /// <summary>
    /// Gets or sets the available quantity (QuantityOnHand - ReservedQuantity).
    /// </summary>
    public decimal AvailableQuantity => QuantityOnHand - ReservedQuantity;

    /// <summary>
    /// Gets or sets the minimum quantity before reorder alert.
    /// </summary>
    public decimal ReorderLevel { get; set; }

    /// <summary>
    /// Gets or sets the maximum quantity to maintain.
    /// </summary>
    public decimal? MaximumLevel { get; set; }

    /// <summary>
    /// Gets or sets the quantity to reorder when stock is low.
    /// </summary>
    public decimal? ReorderQuantity { get; set; }

    /// <summary>
    /// Gets a value indicating whether stock is below reorder level.
    /// </summary>
    public bool IsLowStock => AvailableQuantity <= ReorderLevel;

    public StockLevelValueObject()
    {
    }

    public override bool Equals(object? obj)
    {
        return obj is StockLevelValueObject level &&
               QuantityOnHand == level.QuantityOnHand &&
               ReservedQuantity == level.ReservedQuantity &&
               ReorderLevel == level.ReorderLevel;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(QuantityOnHand, ReservedQuantity, ReorderLevel);
    }

    public override string ToString()
    {
        return $"OnHand: {QuantityOnHand}, Available: {AvailableQuantity}, Reorder: {ReorderLevel}";
    }
}
