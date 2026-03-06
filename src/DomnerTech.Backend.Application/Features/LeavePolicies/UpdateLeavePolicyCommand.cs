using Bas24.CommandQuery;
using DomnerTech.Backend.Application.Abstractions;
using DomnerTech.Backend.Application.DTOs;
using DomnerTech.Backend.Application.DTOs.Leaves.LeavePolicies;

namespace DomnerTech.Backend.Application.Features.LeavePolicies;

public sealed record UpdateLeavePolicyCommand(UpdateLeavePolicyReqDto Dto) :
    IRequest<BaseResponse<bool>>,
    ILogCreator,
    IValidatableRequest;