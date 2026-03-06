using Bas24.CommandQuery;
using DomnerTech.Backend.Application.Abstractions;
using DomnerTech.Backend.Application.DTOs;
using DomnerTech.Backend.Application.DTOs.Leaves.TeamLeave;

namespace DomnerTech.Backend.Application.Features.TeamLeave;

/// <summary>
/// Query to get upcoming team leave (next 30 days).
/// </summary>
public sealed record GetUpcomingTeamLeaveQuery(string Department) :
    IRequest<BaseResponse<IEnumerable<TeamLeaveCalendarDto>>>,
    ILogCreator,
    IValidatableRequest;
