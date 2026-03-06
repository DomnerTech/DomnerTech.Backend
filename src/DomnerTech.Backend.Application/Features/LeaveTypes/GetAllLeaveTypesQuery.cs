using Bas24.CommandQuery;
using DomnerTech.Backend.Application.Abstractions;
using DomnerTech.Backend.Application.DTOs;
using DomnerTech.Backend.Application.DTOs.Leaves.LeaveTypes;

namespace DomnerTech.Backend.Application.Features.LeaveTypes;

/// <summary>
/// Query to get all active leave types.
/// </summary>
public sealed record GetAllLeaveTypesQuery :
    IRequest<BaseResponse<IEnumerable<LeaveTypeDto>>>,
    ILogCreator;
