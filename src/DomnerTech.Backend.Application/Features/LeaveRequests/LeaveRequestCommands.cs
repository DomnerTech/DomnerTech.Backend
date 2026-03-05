using Bas24.CommandQuery;
using DomnerTech.Backend.Application.Abstractions;
using DomnerTech.Backend.Application.DTOs;
using DomnerTech.Backend.Application.DTOs.Leaves.LeaveRequests;

namespace DomnerTech.Backend.Application.Features.LeaveRequests;

public sealed record CreateLeaveRequestCommand(CreateLeaveRequestReqDto Dto) :
    IRequest<BaseResponse<string>>,
    ILogCreator,
    IValidatableRequest;

public sealed record UpdateLeaveRequestCommand(UpdateLeaveRequestReqDto Dto) :
    IRequest<BaseResponse<bool>>,
    ILogCreator,
    IValidatableRequest;

public sealed record CancelLeaveRequestCommand(CancelLeaveRequestReqDto Dto) :
    IRequest<BaseResponse<bool>>,
    ILogCreator,
    IValidatableRequest;

public sealed record GetLeaveRequestByIdQuery(string Id) :
    IRequest<BaseResponse<LeaveRequestDetailDto>>,
    ILogCreator,
    IValidatableRequest;

public sealed record GetMyLeaveRequestsQuery :
    IRequest<BaseResponse<List<LeaveRequestDto>>>,
    ILogCreator;

public sealed record GetLeaveRequestsByStatusQuery(string Status) :
    IRequest<BaseResponse<List<LeaveRequestDto>>>,
    ILogCreator;
