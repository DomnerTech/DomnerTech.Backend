using Bas24.CommandQuery;
using DomnerTech.Backend.Application.Abstractions;
using DomnerTech.Backend.Application.DTOs;
using DomnerTech.Backend.Application.DTOs.Leaves.LeavePolicies;

namespace DomnerTech.Backend.Application.Features.LeavePolicies;

public sealed record CreateLeavePolicyCommand(CreateLeavePolicyReqDto Dto) :
    IRequest<BaseResponse<string>>,
    ILogCreator,
    IValidatableRequest;

public sealed record UpdateLeavePolicyCommand(UpdateLeavePolicyReqDto Dto) :
    IRequest<BaseResponse<bool>>,
    ILogCreator,
    IValidatableRequest;

public sealed record DeleteLeavePolicyCommand(string Id) :
    IRequest<BaseResponse<bool>>,
    ILogCreator,
    IValidatableRequest;

public sealed record GetLeavePolicyByIdQuery(string Id) :
    IRequest<BaseResponse<LeavePolicyDto>>,
    ILogCreator,
    IValidatableRequest;

public sealed record GetActivePoliciesQuery :
    IRequest<BaseResponse<List<LeavePolicyDto>>>,
    ILogCreator;

public sealed record GetPolicyByLeaveTypeQuery(string LeaveTypeId) :
    IRequest<BaseResponse<LeavePolicyDto>>,
    ILogCreator,
    IValidatableRequest;
