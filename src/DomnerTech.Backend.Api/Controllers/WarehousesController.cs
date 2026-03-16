using Bas24.CommandQuery;
using DomnerTech.Backend.Application.DTOs;
using DomnerTech.Backend.Application.DTOs.Inventory;
using DomnerTech.Backend.Application.Features.Warehouses;
using DomnerTech.Backend.Application.IRepo;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DomnerTech.Backend.Api.Controllers;

/// <summary>
/// Controller for managing warehouses.
/// </summary>
public sealed class WarehousesController(
    ICommandQuery commandQuery,
    IErrorMessageLocalizeRepo errorMessageLocalizeRepo) : BaseApiController(errorMessageLocalizeRepo)
{
    /// <summary>
    /// Creates a new warehouse.
    /// </summary>
    /// <remarks>This endpoint requires the 'Warehouse.Write' role.</remarks>
    [HttpPost, Authorize(Roles = "Warehouse.Write")]
    [ProducesResponseType(typeof(BaseResponse<string>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse<string>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(BaseResponse<string>), StatusCodes.Status409Conflict)]
    public async Task<ActionResult<BaseResponse<string>>> CreateWarehouse([FromBody] CreateWarehouseReqDto req)
    {
        var result = await commandQuery.Send(new CreateWarehouseCommand(req), HttpContext.RequestAborted);
        return await ReturnJson(result);
    }

    /// <summary>
    /// Gets all warehouses.
    /// </summary>
    /// <remarks>This endpoint requires the 'Warehouse.Read' role.</remarks>
    [HttpGet, Authorize(Roles = "Warehouse.Read")]
    [ProducesResponseType(typeof(BaseResponse<List<WarehouseDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<BaseResponse<List<WarehouseDto>>>> GetAllWarehouses(
        [FromQuery(Name = "active_only")] bool activeOnly = true)
    {
        var result = await commandQuery.Send(new GetAllWarehousesQuery(activeOnly), HttpContext.RequestAborted);
        return await ReturnJson(result);
    }
}
