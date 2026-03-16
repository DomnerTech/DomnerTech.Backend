using MongoDB.Bson.Serialization.Attributes;

namespace DomnerTech.Backend.Domain.ValueObjects;

/// <summary>
/// Represents a serial number for unique product identification.
/// </summary>
[BsonIgnoreExtraElements]
public sealed class SerialNumberValueObject
{
    /// <summary>
    /// Gets or sets the serial number.
    /// </summary>
    public required string Number { get; set; }

    /// <summary>
    /// Gets or sets the manufacturing date.
    /// </summary>
    public DateTime? ManufactureDate { get; set; }

    /// <summary>
    /// Gets or sets the warranty expiry date.
    /// </summary>
    public DateTime? WarrantyExpiry { get; set; }

    public SerialNumberValueObject()
    {
    }

    public SerialNumberValueObject(string number)
    {
        Number = number;
    }

    public override bool Equals(object? obj)
    {
        return obj is SerialNumberValueObject serial && Number == serial.Number;
    }

    public override int GetHashCode()
    {
        return Number.GetHashCode();
    }

    public override string ToString()
    {
        return Number;
    }
}
