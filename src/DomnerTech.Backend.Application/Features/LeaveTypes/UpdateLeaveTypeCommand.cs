using Bas24.CommandQuery;
using DomnerTech.Backend.Application.Abstractions;
using DomnerTech.Backend.Application.DTOs;
using DomnerTech.Backend.Application.DTOs.Leaves.LeaveTypes;

namespace DomnerTech.Backend.Application.Features.LeaveTypes;

/// <summary>
/// Command to update an existing leave type.
/// </summary>
public sealed record UpdateLeaveTypeCommand(UpdateLeaveTypeReqDto Dto) :
    IRequest<BaseResponse<bool>>,
    ILogCreator,
    IValidatableRequest;
