using DomnerTech.Backend.Application.DTOs;

namespace DomnerTech.Backend.Application.DTOs.Inventory;

/// <summary>
/// DTO for stock information.
/// </summary>
public sealed record StockDto : IBaseDto
{
    public required string Id { get; set; }
    public required string ProductId { get; set; }
    public string? ProductName { get; set; }
    public string? ProductSku { get; set; }
    public string? VariantId { get; set; }
    public required string WarehouseId { get; set; }
    public string? WarehouseName { get; set; }
    public required decimal QuantityOnHand { get; set; }
    public required decimal ReservedQuantity { get; set; }
    public required decimal AvailableQuantity { get; set; }
    public required decimal ReorderLevel { get; set; }
    public decimal? MaximumLevel { get; set; }
    public decimal? ReorderQuantity { get; set; }
    public bool IsLowStock { get; set; }
    public DateTime? LastCountedAt { get; set; }
    public DateTime? LastMovementAt { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
