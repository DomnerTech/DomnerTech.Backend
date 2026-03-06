using Bas24.CommandQuery;
using DomnerTech.Backend.Application.DTOs;
using DomnerTech.Backend.Application.DTOs.Leaves.Reports;
using DomnerTech.Backend.Application.Features.Reports;
using DomnerTech.Backend.Application.IRepo;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DomnerTech.Backend.Api.Controllers;

/// <summary>
/// Controller for leave reports and analytics.
/// </summary>
[Authorize(Roles = "LeaveRequest.Admin")]
public sealed class LeaveReportController(
    ICommandQuery commandQuery,
    IErrorMessageLocalizeRepo errorMessageLocalizeRepo) : BaseApiController(errorMessageLocalizeRepo)
{
    /// <summary>
    /// Generates leave usage report.
    /// </summary>
    /// <remarks>Shows leave allowance, used, and remaining days for all employees or a specific department.</remarks>
    /// <param name="year">The year for the report.</param>
    /// <param name="department">Optional department filter.</param>
    [HttpGet("usage")]
    public async Task<ActionResult<BaseResponse<List<LeaveUsageReportDto>>>> GetLeaveUsageReport(
        [FromQuery] int year,
        [FromQuery] string? department = null)
    {
        var result = await commandQuery.Send(new GetLeaveUsageReportQuery(year, department), HttpContext.RequestAborted);
        return await ReturnJson(result);
    }

    /// <summary>
    /// Gets department-wise leave statistics.
    /// </summary>
    /// <remarks>Provides aggregated leave metrics by department.</remarks>
    /// <param name="department">Optional department filter.</param>
    [HttpGet("department-stats")]
    public async Task<ActionResult<BaseResponse<List<DepartmentLeaveStatsDto>>>> GetDepartmentStats(
        [FromQuery] string? department = null)
    {
        var result = await commandQuery.Send(new GetDepartmentStatsQuery(department), HttpContext.RequestAborted);
        return await ReturnJson(result);
    }

    /// <summary>
    /// Gets leave trend analysis for a year.
    /// </summary>
    /// <remarks>Shows month-by-month leave patterns and trends.</remarks>
    /// <param name="year">The year for trend analysis.</param>
    [HttpGet("trends")]
    public async Task<ActionResult<BaseResponse<List<LeaveTrendDto>>>> GetLeaveTrend([FromQuery] int year)
    {
        var result = await commandQuery.Send(new GetLeaveTrendQuery(year), HttpContext.RequestAborted);
        return await ReturnJson(result);
    }

    /// <summary>
    /// Gets detailed leave summary for a specific employee.
    /// </summary>
    /// <remarks>Shows comprehensive leave information for an employee including all leave types.</remarks>
    /// <param name="employeeId">The employee ID.</param>
    /// <param name="year">The year for the summary.</param>
    [HttpGet("employee-summary/{employeeId}")]
    public async Task<ActionResult<BaseResponse<EmployeeLeaveSummaryDto>>> GetEmployeeLeaveSummary(
        [FromRoute] string employeeId,
        [FromQuery] int year)
    {
        var result = await commandQuery.Send(new GetEmployeeLeaveSummaryQuery(employeeId, year), HttpContext.RequestAborted);
        return await ReturnJson(result);
    }
}
