using DomnerTech.Backend.Domain.Enums;
using DomnerTech.Backend.Domain.ValueObjects;
using MongoDB.Bson;

namespace DomnerTech.Backend.Domain.Entities;

/// <summary>
/// Represents a product in the catalog (Aggregate Root).
/// </summary>
[MongoCollection("products")]
public sealed class ProductEntity : IBaseEntity, ITenantEntity, IAuditEntity, ISoftDeleteEntity
{
    [Sortable(alias: "id", order: 1)]
    public ObjectId Id { get; set; }
    public ObjectId CompanyId { get; set; }

    /// <summary>
    /// Gets or sets the product name (multi-language support).
    /// Key: language code (en, km, vi), Value: product name.
    /// </summary>
    public required Dictionary<string, string> Name { get; set; }

    /// <summary>
    /// Gets or sets the product description (multi-language support).
    /// </summary>
    public Dictionary<string, string>? Description { get; set; }

    /// <summary>
    /// Gets or sets the product SKU (Stock Keeping Unit).
    /// </summary>
    public required SkuValueObject Sku { get; set; }

    /// <summary>
    /// Gets or sets the product barcodes.
    /// </summary>
    public List<BarcodeValueObject>? Barcodes { get; set; }

    /// <summary>
    /// Gets or sets the category ID.
    /// </summary>
    public required ObjectId CategoryId { get; set; }

    /// <summary>
    /// Gets or sets the brand ID.
    /// </summary>
    public ObjectId? BrandId { get; set; }

    /// <summary>
    /// Gets or sets the product prices for different types and currencies.
    /// </summary>
    public required List<ProductPriceValueObject> Prices { get; set; }

    /// <summary>
    /// Gets or sets the cost price for profit calculation.
    /// </summary>
    public decimal? CostPrice { get; set; }

    /// <summary>
    /// Gets or sets the unit of measure.
    /// </summary>
    public UnitOfMeasure UnitOfMeasure { get; set; } = UnitOfMeasure.Piece;

    /// <summary>
    /// Gets or sets the product attributes (e.g., color, size, material).
    /// </summary>
    public List<ProductAttributeValueObject>? Attributes { get; set; }

    /// <summary>
    /// Gets or sets the product images.
    /// </summary>
    public List<string>? Images { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether this product has variants.
    /// </summary>
    public bool HasVariants { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether this product is a bundle/kit.
    /// </summary>
    public bool IsBundle { get; set; }

    /// <summary>
    /// Gets or sets the product lifecycle status.
    /// </summary>
    [Sortable(alias: "status", order: 2)]
    public ProductStatus Status { get; set; } = ProductStatus.Draft;

    /// <summary>
    /// Gets or sets a value indicating whether stock tracking is enabled.
    /// </summary>
    public bool TrackInventory { get; set; } = true;

    /// <summary>
    /// Gets or sets a value indicating whether batch/lot tracking is enabled.
    /// </summary>
    public bool TrackBatchLot { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether serial number tracking is enabled.
    /// </summary>
    public bool TrackSerialNumber { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether this product is taxable.
    /// </summary>
    public bool IsTaxable { get; set; } = true;

    /// <summary>
    /// Gets or sets the tax rate percentage.
    /// </summary>
    public decimal? TaxRate { get; set; }

    /// <summary>
    /// Gets or sets the product weight in kilograms.
    /// </summary>
    public decimal? Weight { get; set; }

    /// <summary>
    /// Gets or sets the product dimensions (Length x Width x Height in cm).
    /// </summary>
    public string? Dimensions { get; set; }

    /// <summary>
    /// Gets or sets the supplier ID.
    /// </summary>
    public ObjectId? SupplierId { get; set; }

    /// <summary>
    /// Gets or sets the manufacturer name.
    /// </summary>
    public string? Manufacturer { get; set; }

    /// <summary>
    /// Gets or sets tags for search and categorization.
    /// </summary>
    public List<string>? Tags { get; set; }

    /// <summary>
    /// Gets or sets metadata for additional information.
    /// </summary>
    public Dictionary<string, string>? Metadata { get; set; }

    /// <summary>
    /// Gets or sets the warehouse IDs where this product is available.
    /// </summary>
    public List<ObjectId>? WarehouseIds { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public ObjectId? UpdatedBy { get; set; }
    public ObjectId? DeletedBy { get; set; }
    public bool IsDeleted { get; set; }
}
