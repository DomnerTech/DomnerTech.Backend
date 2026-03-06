using Bas24.CommandQuery;
using DomnerTech.Backend.Application.Abstractions;
using DomnerTech.Backend.Application.DTOs;
using DomnerTech.Backend.Application.DTOs.Leaves.LeavePolicies;

namespace DomnerTech.Backend.Application.Features.LeavePolicies;

public sealed record GetLeavePolicyByIdQuery(string Id) :
    IRequest<BaseResponse<LeavePolicyDto>>,
    ILogCreator,
    IValidatableRequest;