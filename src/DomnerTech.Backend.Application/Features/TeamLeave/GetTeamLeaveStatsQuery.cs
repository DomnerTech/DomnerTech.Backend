using Bas24.CommandQuery;
using DomnerTech.Backend.Application.Abstractions;
using DomnerTech.Backend.Application.DTOs;
using DomnerTech.Backend.Application.DTOs.Leaves.TeamLeave;

namespace DomnerTech.Backend.Application.Features.TeamLeave;

/// <summary>
/// Query to get team leave statistics.
/// </summary>
public sealed record GetTeamLeaveStatsQuery(string Department) :
    IRequest<BaseResponse<TeamLeaveStatsDto>>,
    ILogCreator,
    IValidatableRequest;