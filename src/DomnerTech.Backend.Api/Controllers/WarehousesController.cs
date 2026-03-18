using Bas24.CommandQuery;
using DomnerTech.Backend.Application.DTOs;
using DomnerTech.Backend.Application.DTOs.Inventory;
using DomnerTech.Backend.Application.Features.Warehouses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DomnerTech.Backend.Api.Controllers;

/// <summary>
/// Controller for managing warehouses.
/// </summary>
public sealed class WarehousesController(ICommandQuery commandQuery) : BaseApiController(commandQuery)
{
    /// <summary>
    /// Creates a new warehouse.
    /// </summary>
    /// <remarks>This endpoint requires the 'Warehouse.Write' role.</remarks>
    [HttpPost, Authorize(Roles = "Warehouse.Write")]
    public async Task<ActionResult<BaseResponse<string>>> CreateWarehouse([FromBody] CreateWarehouseReqDto req)
    {
        var result = await _commandQuery.Send(new CreateWarehouseCommand(req), HttpContext.RequestAborted);
        return await ReturnJson(result);
    }

    /// <summary>
    /// Gets all warehouses.
    /// </summary>
    /// <remarks>This endpoint requires the 'Warehouse.Read' role.</remarks>
    [HttpGet, Authorize(Roles = "Warehouse.Read")]
    public async Task<ActionResult<BaseResponse<List<WarehouseDto>>>> GetAllWarehouses(
        [FromQuery(Name = "active_only")] bool activeOnly = true)
    {
        var result = await _commandQuery.Send(new GetAllWarehousesQuery(activeOnly), HttpContext.RequestAborted);
        return await ReturnJson(result);
    }
}
