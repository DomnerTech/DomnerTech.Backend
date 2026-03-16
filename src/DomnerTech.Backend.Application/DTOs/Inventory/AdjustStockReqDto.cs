using DomnerTech.Backend.Application.DTOs;
using DomnerTech.Backend.Domain.Enums;

namespace DomnerTech.Backend.Application.DTOs.Inventory;

/// <summary>
/// DTO for adjusting stock.
/// </summary>
public sealed record AdjustStockReqDto
{
    /// <summary>
    /// Product ID.
    /// </summary>
    public required string ProductId { get; set; }

    /// <summary>
    /// Warehouse ID.
    /// </summary>
    public required string WarehouseId { get; set; }

    /// <summary>
    /// Product variant ID (if applicable).
    /// </summary>
    public string? VariantId { get; set; }

    /// <summary>
    /// Quantity to adjust (positive for increase, negative for decrease).
    /// </summary>
    public required decimal Quantity { get; set; }

    /// <summary>
    /// Reason for adjustment.
    /// </summary>
    public required InventoryAdjustmentReason Reason { get; set; }

    /// <summary>
    /// Batch/lot number.
    /// </summary>
    public string? BatchLotNumber { get; set; }

    /// <summary>
    /// Serial number.
    /// </summary>
    public string? SerialNumber { get; set; }

    /// <summary>
    /// Reference document ID.
    /// </summary>
    public string? ReferenceId { get; set; }

    /// <summary>
    /// Notes or remarks.
    /// </summary>
    public string? Notes { get; set; }
}
