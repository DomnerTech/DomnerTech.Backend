using Bas24.CommandQuery;
using DomnerTech.Backend.Application.Abstractions;
using DomnerTech.Backend.Application.DTOs;

namespace DomnerTech.Backend.Application.Features.LeaveTypes;

/// <summary>
/// Command to delete a leave type.
/// </summary>
public sealed record DeleteLeaveTypeCommand(string Id) :
    IRequest<BaseResponse<bool>>,
    ILogCreator,
    IValidatableRequest;
