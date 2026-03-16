using MongoDB.Bson.Serialization.Attributes;

namespace DomnerTech.Backend.Domain.ValueObjects;

/// <summary>
/// Represents a batch or lot number for product tracking.
/// </summary>
[BsonIgnoreExtraElements]
public sealed class BatchLotValueObject
{
    /// <summary>
    /// Gets or sets the batch/lot number.
    /// </summary>
    public required string Number { get; set; }

    /// <summary>
    /// Gets or sets the manufacturing date.
    /// </summary>
    public DateTime? ManufactureDate { get; set; }

    /// <summary>
    /// Gets or sets the expiry date.
    /// </summary>
    public DateTime? ExpiryDate { get; set; }

    /// <summary>
    /// Gets or sets the supplier lot reference.
    /// </summary>
    public string? SupplierLotReference { get; set; }

    /// <summary>
    /// Gets a value indicating whether this batch is expired.
    /// </summary>
    public bool IsExpired => ExpiryDate.HasValue && ExpiryDate.Value < DateTime.UtcNow;

    public BatchLotValueObject()
    {
    }

    public BatchLotValueObject(string number)
    {
        Number = number;
    }

    public override bool Equals(object? obj)
    {
        return obj is BatchLotValueObject batch && Number == batch.Number;
    }

    public override int GetHashCode()
    {
        return Number.GetHashCode();
    }

    public override string ToString()
    {
        return $"{Number} {(IsExpired ? "(Expired)" : "")}";
    }
}
