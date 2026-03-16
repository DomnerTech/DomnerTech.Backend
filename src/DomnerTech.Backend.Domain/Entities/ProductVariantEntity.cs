using DomnerTech.Backend.Domain.ValueObjects;
using MongoDB.Bson;

namespace DomnerTech.Backend.Domain.Entities;

/// <summary>
/// Represents a product variant (e.g., different sizes, colors, or configurations).
/// </summary>
[MongoCollection("productVariants")]
public sealed class ProductVariantEntity : IBaseEntity, ITenantEntity, IAuditEntity, ISoftDeleteEntity
{
    [Sortable(alias: "id", order: 1)]
    public ObjectId Id { get; set; }
    public ObjectId CompanyId { get; set; }

    /// <summary>
    /// Gets or sets the parent product ID.
    /// </summary>
    public required ObjectId ProductId { get; set; }

    /// <summary>
    /// Gets or sets the variant name (e.g., "Red - XL", "256GB - Black").
    /// </summary>
    public required Dictionary<string, string> Name { get; set; }

    /// <summary>
    /// Gets or sets the variant SKU.
    /// </summary>
    public required SkuValueObject Sku { get; set; }

    /// <summary>
    /// Gets or sets the variant barcodes.
    /// </summary>
    public List<BarcodeValueObject>? Barcodes { get; set; }

    /// <summary>
    /// Gets or sets the variant-specific attributes (e.g., Color: Red, Size: XL).
    /// </summary>
    public required List<ProductAttributeValueObject> Attributes { get; set; }

    /// <summary>
    /// Gets or sets the variant-specific prices (can override product prices).
    /// </summary>
    public List<ProductPriceValueObject>? Prices { get; set; }

    /// <summary>
    /// Gets or sets the variant-specific cost price.
    /// </summary>
    public decimal? CostPrice { get; set; }

    /// <summary>
    /// Gets or sets the variant images.
    /// </summary>
    public List<string>? Images { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether this variant is active.
    /// </summary>
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// Gets or sets the display order for sorting.
    /// </summary>
    public int DisplayOrder { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public ObjectId? UpdatedBy { get; set; }
    public ObjectId? DeletedBy { get; set; }
    public bool IsDeleted { get; set; }
}
