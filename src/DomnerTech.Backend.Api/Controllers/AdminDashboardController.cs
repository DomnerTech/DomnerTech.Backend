using Bas24.CommandQuery;
using DomnerTech.Backend.Application.DTOs;
using DomnerTech.Backend.Application.DTOs.Leaves.Dashboard;
using DomnerTech.Backend.Application.Features.Dashboard;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DomnerTech.Backend.Api.Controllers;

/// <summary>
/// Controller for admin dashboard.
/// </summary>
[Authorize(Roles = "LeaveRequest.Admin")]
public sealed class AdminDashboardController(ICommandQuery commandQuery) : BaseApiController(commandQuery)
{
    /// <summary>
    /// Gets admin dashboard statistics.
    /// </summary>
    /// <remarks>Provides overview metrics for HR/Admin including pending approvals, employees on leave, and statistics.</remarks>
    [HttpGet("stats")]
    public async Task<ActionResult<BaseResponse<AdminDashboardStatsDto>>> GetDashboardStats()
    {
        var result = await _commandQuery.Send(new GetAdminDashboardStatsQuery(), HttpContext.RequestAborted);
        return await ReturnJson(result);
    }

    /// <summary>
    /// Gets list of employees currently on leave.
    /// </summary>
    /// <remarks>Shows all employees who are on approved leave today.</remarks>
    [HttpGet("employees-on-leave")]
    public async Task<ActionResult<BaseResponse<List<EmployeeOnLeaveDto>>>> GetEmployeesOnLeave()
    {
        var result = await _commandQuery.Send(new GetEmployeesOnLeaveQuery(), HttpContext.RequestAborted);
        return await ReturnJson(result);
    }

    /// <summary>
    /// Gets upcoming leaves.
    /// </summary>
    /// <remarks>Shows approved leaves starting in the next N days (default: 30 days).</remarks>
    /// <param name="days">Number of days to look ahead (default: 30).</param>
    [HttpGet("upcoming-leaves")]
    public async Task<ActionResult<BaseResponse<List<UpcomingLeaveDto>>>> GetUpcomingLeaves([FromQuery] int days = 30)
    {
        var result = await _commandQuery.Send(new GetUpcomingLeavesQuery(days), HttpContext.RequestAborted);
        return await ReturnJson(result);
    }

    /// <summary>
    /// Gets pending approvals summary.
    /// </summary>
    /// <remarks>Shows all leave requests waiting for approval with details including how long they've been pending.</remarks>
    [HttpGet("pending-approvals")]
    public async Task<ActionResult<BaseResponse<List<PendingApprovalSummaryDto>>>> GetPendingApprovals()
    {
        var result = await _commandQuery.Send(new GetPendingApprovalsSummaryQuery(), HttpContext.RequestAborted);
        return await ReturnJson(result);
    }
}
