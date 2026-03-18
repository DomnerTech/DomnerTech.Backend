using Bas24.CommandQuery;
using DomnerTech.Backend.Application.DTOs;
using DomnerTech.Backend.Application.DTOs.Leaves.TeamLeave;
using DomnerTech.Backend.Application.Features.TeamLeave;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DomnerTech.Backend.Api.Controllers;

/// <summary>
/// Controller for team leave visibility and management.
/// </summary>
public sealed class TeamLeaveController(ICommandQuery commandQuery) : BaseApiController(commandQuery)
{
    /// <summary>
    /// Gets team leave calendar for a date range.
    /// </summary>
    /// <remarks>This endpoint requires the 'LeaveRequest.Admin' role. Managers can view their team's leave schedule.</remarks>
    /// <param name="department">The department name.</param>
    /// <param name="startDate">Start date for the calendar view.</param>
    /// <param name="endDate">End date for the calendar view.</param>
    /// <returns>List of team leave calendar entries.</returns>
    [HttpGet("calendar"), Authorize(Roles = "LeaveRequest.Admin")]
    public async Task<ActionResult<BaseResponse<List<TeamLeaveCalendarDto>>>> GetTeamLeaveCalendar(
        [FromQuery] string department,
        [FromQuery(Name = "start_date")] DateTime startDate,
        [FromQuery(Name = "end_date")] DateTime endDate)
    {
        var result = await _commandQuery.Send(new GetTeamLeaveCalendarQuery(department, startDate, endDate), HttpContext.RequestAborted);
        return await ReturnJson(result);
    }

    /// <summary>
    /// Checks for team leave conflicts within a date range.
    /// </summary>
    /// <remarks>This endpoint requires the 'LeaveRequest.Admin' role. Detects when too many team members are on leave simultaneously.</remarks>
    /// <param name="req">The conflict check request with department, date range, and max employees allowed.</param>
    /// <returns>List of dates with conflicts.</returns>
    [HttpPost("check-conflicts"), Authorize(Roles = "LeaveRequest.Admin")]
    public async Task<ActionResult<BaseResponse<List<TeamLeaveConflictDto>>>> CheckTeamLeaveConflicts(
        [FromBody] CheckTeamLeaveConflictReqDto req)
    {
        var result = await _commandQuery.Send(new CheckTeamLeaveConflictQuery(req), HttpContext.RequestAborted);
        return await ReturnJson(result);
    }

    /// <summary>
    /// Gets team leave statistics for a department.
    /// </summary>
    /// <remarks>This endpoint requires the 'LeaveRequest.Admin' role. Provides overview of team leave metrics.</remarks>
    /// <param name="department">The department name.</param>
    /// <returns>Team leave statistics.</returns>
    [HttpGet("stats/{department}"), Authorize(Roles = "LeaveRequest.Admin")]
    public async Task<ActionResult<BaseResponse<TeamLeaveStatsDto>>> GetTeamLeaveStats([FromRoute] string department)
    {
        var result = await _commandQuery.Send(new GetTeamLeaveStatsQuery(department), HttpContext.RequestAborted);
        return await ReturnJson(result);
    }

    /// <summary>
    /// Gets upcoming team leave for the next 30 days.
    /// </summary>
    /// <remarks>This endpoint requires the 'LeaveRequest.Admin' role. Helps managers plan for upcoming absences.</remarks>
    /// <param name="department">The department name.</param>
    /// <returns>List of upcoming leave.</returns>
    [HttpGet("upcoming/{department}"), Authorize(Roles = "LeaveRequest.Admin")]
    public async Task<ActionResult<BaseResponse<List<TeamLeaveCalendarDto>>>> GetUpcomingTeamLeave([FromRoute] string department)
    {
        var result = await _commandQuery.Send(new GetUpcomingTeamLeaveQuery(department), HttpContext.RequestAborted);
        return await ReturnJson(result);
    }
}
