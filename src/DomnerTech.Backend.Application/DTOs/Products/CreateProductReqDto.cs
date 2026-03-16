using DomnerTech.Backend.Application.DTOs;
using DomnerTech.Backend.Domain.Enums;
using MongoDB.Bson;

namespace DomnerTech.Backend.Application.DTOs.Products;

/// <summary>
/// DTO for creating a new product.
/// </summary>
public sealed record CreateProductReqDto
{
    /// <summary>
    /// Product name in multiple languages.
    /// </summary>
    public required Dictionary<string, string> Name { get; set; }

    /// <summary>
    /// Product description in multiple languages.
    /// </summary>
    public Dictionary<string, string>? Description { get; set; }

    /// <summary>
    /// Product SKU (leave null for auto-generation).
    /// </summary>
    public string? Sku { get; set; }

    /// <summary>
    /// Category ID.
    /// </summary>
    public required string CategoryId { get; set; }

    /// <summary>
    /// Brand ID.
    /// </summary>
    public string? BrandId { get; set; }

    /// <summary>
    /// Product prices for different types and currencies.
    /// </summary>
    public required List<ProductPriceDto> Prices { get; set; }

    /// <summary>
    /// Cost price.
    /// </summary>
    public decimal? CostPrice { get; set; }

    /// <summary>
    /// Unit of measure.
    /// </summary>
    public UnitOfMeasure UnitOfMeasure { get; set; } = UnitOfMeasure.Piece;

    /// <summary>
    /// Product attributes.
    /// </summary>
    public List<ProductAttributeDto>? Attributes { get; set; }

    /// <summary>
    /// Product barcodes.
    /// </summary>
    public List<BarcodeDto>? Barcodes { get; set; }

    /// <summary>
    /// Product images URLs.
    /// </summary>
    public List<string>? Images { get; set; }

    /// <summary>
    /// Track inventory flag.
    /// </summary>
    public bool TrackInventory { get; set; } = true;

    /// <summary>
    /// Track batch/lot flag.
    /// </summary>
    public bool TrackBatchLot { get; set; }

    /// <summary>
    /// Track serial number flag.
    /// </summary>
    public bool TrackSerialNumber { get; set; }

    /// <summary>
    /// Taxable flag.
    /// </summary>
    public bool IsTaxable { get; set; } = true;

    /// <summary>
    /// Tax rate percentage.
    /// </summary>
    public decimal? TaxRate { get; set; }

    /// <summary>
    /// Product weight in kg.
    /// </summary>
    public decimal? Weight { get; set; }

    /// <summary>
    /// Product dimensions.
    /// </summary>
    public string? Dimensions { get; set; }

    /// <summary>
    /// Tags.
    /// </summary>
    public List<string>? Tags { get; set; }

    /// <summary>
    /// Warehouse IDs where product is available.
    /// </summary>
    public List<string>? WarehouseIds { get; set; }
}

/// <summary>
/// DTO for product price.
/// </summary>
public sealed record ProductPriceDto
{
    public required PriceType PriceType { get; set; }
    public required CurrencyCode Currency { get; set; }
    public required decimal Amount { get; set; }
    public DateTime? EffectiveFrom { get; set; }
    public DateTime? EffectiveTo { get; set; }
}

/// <summary>
/// DTO for product attribute.
/// </summary>
public sealed record ProductAttributeDto
{
    public required string Name { get; set; }
    public required string Value { get; set; }
}

/// <summary>
/// DTO for barcode.
/// </summary>
public sealed record BarcodeDto
{
    public required string Value { get; set; }
    public required string Type { get; set; }
}
