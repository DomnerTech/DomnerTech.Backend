using Bas24.CommandQuery;
using DomnerTech.Backend.Application.Abstractions;
using DomnerTech.Backend.Application.DTOs;
using DomnerTech.Backend.Application.DTOs.Leaves.TeamLeave;

namespace DomnerTech.Backend.Application.Features.TeamLeave;

/// <summary>
/// Query to check for team leave conflicts.
/// </summary>
public sealed record CheckTeamLeaveConflictQuery(CheckTeamLeaveConflictReqDto Dto) :
    IRequest<BaseResponse<IEnumerable<TeamLeaveConflictDto>>>,
    ILogCreator,
    IValidatableRequest;