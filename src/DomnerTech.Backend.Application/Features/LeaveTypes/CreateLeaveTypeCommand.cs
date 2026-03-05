using Bas24.CommandQuery;
using DomnerTech.Backend.Application.Abstractions;
using DomnerTech.Backend.Application.DTOs;
using DomnerTech.Backend.Application.DTOs.Leaves.LeaveTypes;

namespace DomnerTech.Backend.Application.Features.LeaveTypes;

/// <summary>
/// Command to create a new leave type.
/// </summary>
public sealed record CreateLeaveTypeCommand(CreateLeaveTypeReqDto Dto) :
    IRequest<BaseResponse<string>>,
    ILogCreator,
    IValidatableRequest;
