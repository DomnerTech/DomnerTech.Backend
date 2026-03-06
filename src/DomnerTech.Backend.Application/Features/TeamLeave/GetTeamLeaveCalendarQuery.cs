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