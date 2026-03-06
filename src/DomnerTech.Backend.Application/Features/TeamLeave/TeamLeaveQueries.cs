using Bas24.CommandQuery;
using DomnerTech.Backend.Application.Abstractions;
using DomnerTech.Backend.Application.DTOs;
using DomnerTech.Backend.Application.DTOs.Leaves.TeamLeave;

namespace DomnerTech.Backend.Application.Features.TeamLeave;

/// <summary>
/// Query to get team leave calendar for a date range.
/// </summary>
public sealed record GetTeamLeaveCalendarQuery(string Department, DateTime StartDate, DateTime EndDate) :
    IRequest<BaseResponse<IEnumerable<TeamLeaveCalendarDto>>>,
    ILogCreator,
    IValidatableRequest;

/// <summary>
/// Query to check for team leave conflicts.
/// </summary>
public sealed record CheckTeamLeaveConflictQuery(CheckTeamLeaveConflictReqDto Dto) :
    IRequest<BaseResponse<List<TeamLeaveConflictDto>>>,
    ILogCreator,
    IValidatableRequest;

/// <summary>
/// Query to get team leave statistics.
/// </summary>
public sealed record GetTeamLeaveStatsQuery(string Department) :
    IRequest<BaseResponse<TeamLeaveStatsDto>>,
    ILogCreator,
    IValidatableRequest;

/// <summary>
/// Query to get upcoming team leave (next 30 days).
/// </summary>
public sealed record GetUpcomingTeamLeaveQuery(string Department) :
    IRequest<BaseResponse<IEnumerable<TeamLeaveCalendarDto>>>,
    ILogCreator,
    IValidatableRequest;
