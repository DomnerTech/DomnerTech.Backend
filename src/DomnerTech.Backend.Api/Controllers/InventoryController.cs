using Bas24.CommandQuery;
using DomnerTech.Backend.Application.DTOs;
using DomnerTech.Backend.Application.DTOs.Inventory;
using DomnerTech.Backend.Application.Features.Inventory;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DomnerTech.Backend.Api.Controllers;

/// <summary>
/// Controller for managing inventory and stock operations.
/// </summary>
public sealed class InventoryController(ICommandQuery commandQuery) : BaseApiController(commandQuery)
{
    /// <summary>
    /// Adjusts stock quantity for a product in a warehouse.
    /// </summary>
    /// <remarks>
    /// This endpoint requires the 'Inventory.Write' role. 
    /// Use positive quantity for increase, negative for decrease.
    /// All adjustments are recorded in stock movement history.
    /// </remarks>
    [HttpPost("adjust"), Authorize(Roles = "Inventory.Write")]
    public async Task<ActionResult<BaseResponse<bool>>> AdjustStock([FromBody] AdjustStockReqDto req)
    {
        var result = await _commandQuery.Send(new AdjustStockCommand(req), HttpContext.RequestAborted);
        return await ReturnJson(result);
    }

    /// <summary>
    /// Reserves stock for an order.
    /// </summary>
    /// <remarks>
    /// This endpoint requires the 'Inventory.Write' role. 
    /// Reserved stock is not available for other orders.
    /// Reservations can have an expiry date.
    /// </remarks>
    [HttpPost("reserve"), Authorize(Roles = "Inventory.Write")]
    public async Task<ActionResult<BaseResponse<string>>> ReserveStock(
        [FromQuery(Name = "product_id")] string productId,
        [FromQuery(Name = "warehouse_id")] string warehouseId,
        [FromQuery(Name = "quantity")] decimal quantity,
        [FromQuery(Name = "order_id")] string orderId,
        [FromQuery(Name = "variant_id")] string? variantId = null,
        [FromQuery(Name = "expires_at")] DateTime? expiresAt = null)
    {
        var result = await _commandQuery.Send(
            new ReserveStockCommand(productId, warehouseId, quantity, orderId, variantId, expiresAt),
            HttpContext.RequestAborted);
        return await ReturnJson(result);
    }

    /// <summary>
    /// Creates a stock transfer between warehouses.
    /// </summary>
    /// <remarks>
    /// This endpoint requires the 'Inventory.Write' role. 
    /// Transfer must be approved before stock is moved.
    /// </remarks>
    [HttpPost("transfer"), Authorize(Roles = "Inventory.Write")]
    public async Task<ActionResult<BaseResponse<string>>> CreateStockTransfer([FromBody] CreateStockTransferReqDto req)
    {
        var result = await _commandQuery.Send(new CreateStockTransferCommand(req), HttpContext.RequestAborted);
        return await ReturnJson(result);
    }

    /// <summary>
    /// Gets stock information for a product.
    /// </summary>
    /// <remarks>This endpoint requires the 'Inventory.Read' role. Returns stock levels across all or specific warehouse.</remarks>
    [HttpGet("stock/{productId}"), Authorize(Roles = "Inventory.Read")]
    public async Task<ActionResult<BaseResponse<List<StockDto>>>> GetStockByProduct(
        [FromRoute] string productId,
        [FromQuery(Name = "warehouse_id")] string? warehouseId = null,
        [FromQuery(Name = "variant_id")] string? variantId = null)
    {
        var result = await _commandQuery.Send(
            new GetStockByProductQuery(productId, warehouseId, variantId),
            HttpContext.RequestAborted);
        return await ReturnJson(result);
    }

    /// <summary>
    /// Gets low stock items that need reordering.
    /// </summary>
    /// <remarks>This endpoint requires the 'Inventory.Read' role. Returns products below reorder level.</remarks>
    [HttpGet("low-stock"), Authorize(Roles = "Inventory.Read")]
    public async Task<ActionResult<BaseResponse<List<StockDto>>>> GetLowStockItems(
        [FromQuery(Name = "warehouse_id")] string? warehouseId = null)
    {
        var result = await _commandQuery.Send(new GetLowStockItemsQuery(warehouseId), HttpContext.RequestAborted);
        return await ReturnJson(result);
    }
}
