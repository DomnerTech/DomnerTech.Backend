using MongoDB.Bson.Serialization.Attributes;

namespace DomnerTech.Backend.Domain.ValueObjects;

/// <summary>
/// Represents a dynamic product attribute (e.g., color, size, material).
/// </summary>
[BsonIgnoreExtraElements]
public sealed class ProductAttributeValueObject
{
    /// <summary>
    /// Gets or sets the attribute name (e.g., "Color", "Size", "Material").
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    /// Gets or sets the attribute value (e.g., "Red", "XL", "Cotton").
    /// </summary>
    public required string Value { get; set; }

    public ProductAttributeValueObject()
    {
    }

    public ProductAttributeValueObject(string name, string value)
    {
        Name = name;
        Value = value;
    }

    public override bool Equals(object? obj)
    {
        return obj is ProductAttributeValueObject attribute &&
               Name == attribute.Name &&
               Value == attribute.Value;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Name, Value);
    }

    public override string ToString()
    {
        return $"{Name}: {Value}";
    }
}
