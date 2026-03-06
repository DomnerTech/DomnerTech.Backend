using Bas24.CommandQuery;
using DomnerTech.Backend.Application.Abstractions;
using DomnerTech.Backend.Application.DTOs;
using DomnerTech.Backend.Application.DTOs.Leaves.LeaveTypes;

namespace DomnerTech.Backend.Application.Features.LeaveTypes;

/// <summary>
/// Query to get a leave type by ID.
/// </summary>
public sealed record GetLeaveTypeByIdQuery(string Id) :
    IRequest<BaseResponse<LeaveTypeDto?>>,
    ILogCreator,
    IValidatableRequest;
