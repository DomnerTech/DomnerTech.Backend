using DomnerTech.Backend.Application.DTOs;
using DomnerTech.Backend.Domain.Enums;

namespace DomnerTech.Backend.Application.DTOs.Inventory;

/// <summary>
/// DTO for creating a stock transfer.
/// </summary>
public sealed record CreateStockTransferReqDto
{
    /// <summary>
    /// Source warehouse ID.
    /// </summary>
    public required string FromWarehouseId { get; set; }

    /// <summary>
    /// Destination warehouse ID.
    /// </summary>
    public required string ToWarehouseId { get; set; }

    /// <summary>
    /// Product ID.
    /// </summary>
    public required string ProductId { get; set; }

    /// <summary>
    /// Product variant ID (if applicable).
    /// </summary>
    public string? VariantId { get; set; }

    /// <summary>
    /// Quantity to transfer.
    /// </summary>
    public required decimal Quantity { get; set; }

    /// <summary>
    /// Notes or remarks.
    /// </summary>
    public string? Notes { get; set; }
}
