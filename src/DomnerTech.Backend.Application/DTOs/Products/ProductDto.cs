using DomnerTech.Backend.Application.DTOs;
using DomnerTech.Backend.Domain.Enums;

namespace DomnerTech.Backend.Application.DTOs.Products;

/// <summary>
/// DTO for product response.
/// </summary>
public sealed record ProductDto : IBaseDto
{
    public required string Id { get; set; }
    public required Dictionary<string, string> Name { get; set; }
    public Dictionary<string, string>? Description { get; set; }
    public required string Sku { get; set; }
    public List<BarcodeDto>? Barcodes { get; set; }
    public required string CategoryId { get; set; }
    public string? CategoryName { get; set; }
    public string? BrandId { get; set; }
    public string? BrandName { get; set; }
    public required List<ProductPriceDto> Prices { get; set; }
    public decimal? CostPrice { get; set; }
    public UnitOfMeasure UnitOfMeasure { get; set; }
    public List<ProductAttributeDto>? Attributes { get; set; }
    public List<string>? Images { get; set; }
    public bool HasVariants { get; set; }
    public bool IsBundle { get; set; }
    public ProductStatus Status { get; set; }
    public bool TrackInventory { get; set; }
    public bool TrackBatchLot { get; set; }
    public bool TrackSerialNumber { get; set; }
    public bool IsTaxable { get; set; }
    public decimal? TaxRate { get; set; }
    public decimal? Weight { get; set; }
    public string? Dimensions { get; set; }
    public List<string>? Tags { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
