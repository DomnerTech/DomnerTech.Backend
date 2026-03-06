using Bas24.CommandQuery;
using DomnerTech.Backend.Application.DTOs;
using DomnerTech.Backend.Application.DTOs.Leaves.LeaveBalances;
using DomnerTech.Backend.Application.Features.LeaveBalances;
using DomnerTech.Backend.Application.IRepo;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DomnerTech.Backend.Api.Controllers;

/// <summary>
/// Controller for managing leave balances.
/// </summary>
public sealed class LeaveBalanceController(
    ICommandQuery commandQuery,
    IErrorMessageLocalizeRepo errorMessageLocalizeRepo) : BaseApiController(errorMessageLocalizeRepo)
{
    /// <summary>
    /// Initializes leave balance for an employee.
    /// </summary>
    [HttpPost, Authorize(Roles = "LeaveBalance.Write")]
    public async Task<ActionResult<BaseResponse<string>>> InitializeLeaveBalance([FromBody] InitializeLeaveBalanceReqDto req)
    {
        var result = await commandQuery.Send(new InitializeLeaveBalanceCommand(req), HttpContext.RequestAborted);
        return await ReturnJson(result);
    }

    /// <summary>
    /// Adjusts an employee's leave balance.
    /// </summary>
    [HttpPost("adjust"), Authorize(Roles = "LeaveBalance.Write")]
    public async Task<ActionResult<BaseResponse<bool>>> AdjustLeaveBalance([FromBody] AdjustLeaveBalanceReqDto req)
    {
        var result = await commandQuery.Send(new AdjustLeaveBalanceCommand(req), HttpContext.RequestAborted);
        return await ReturnJson(result);
    }

    /// <summary>
    /// Retrieves the current user's leave balances.
    /// </summary>
    [HttpGet("my"), Authorize(Roles = "LeaveBalance.Read")]
    public async Task<ActionResult<BaseResponse<IEnumerable<LeaveBalanceSummaryDto>>>> GetMyLeaveBalances([FromQuery] int year)
    {
        var result = await commandQuery.Send(new GetMyLeaveBalancesQuery(year), HttpContext.RequestAborted);
        return await ReturnJson(result);
    }

    /// <summary>
    /// Retrieves an employee's leave balances.
    /// </summary>
    [HttpGet("employee/{employeeId}"), Authorize(Roles = "LeaveBalance.Write")]
    public async Task<ActionResult<BaseResponse<IEnumerable<LeaveBalanceSummaryDto>>>> GetEmployeeLeaveBalances([FromRoute] string employeeId, [FromQuery] int year)
    {
        var result = await commandQuery.Send(new GetEmployeeLeaveBalancesQuery(employeeId, year), HttpContext.RequestAborted);
        return await ReturnJson(result);
    }
}
