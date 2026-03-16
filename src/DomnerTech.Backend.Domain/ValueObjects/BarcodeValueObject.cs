using MongoDB.Bson.Serialization.Attributes;

namespace DomnerTech.Backend.Domain.ValueObjects;

/// <summary>
/// Represents a barcode identifier.
/// </summary>
[BsonIgnoreExtraElements]
public sealed class BarcodeValueObject
{
    /// <summary>
    /// Gets or sets the barcode value.
    /// </summary>
    public required string Value { get; set; }

    /// <summary>
    /// Gets or sets the barcode type (e.g., EAN-13, UPC, QR Code).
    /// </summary>
    public required string Type { get; set; }

    public BarcodeValueObject()
    {
    }

    public BarcodeValueObject(string value, string type)
    {
        Value = value;
        Type = type;
    }

    public override bool Equals(object? obj)
    {
        return obj is BarcodeValueObject barcode &&
               Value == barcode.Value &&
               Type == barcode.Type;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Value, Type);
    }

    public override string ToString()
    {
        return $"{Type}: {Value}";
    }
}
