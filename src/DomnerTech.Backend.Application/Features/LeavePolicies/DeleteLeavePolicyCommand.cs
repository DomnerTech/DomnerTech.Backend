using Bas24.CommandQuery;
using DomnerTech.Backend.Application.Abstractions;
using DomnerTech.Backend.Application.DTOs;

namespace DomnerTech.Backend.Application.Features.LeavePolicies;

public sealed record DeleteLeavePolicyCommand(string Id) :
    IRequest<BaseResponse<bool>>,
    ILogCreator,
    IValidatableRequest;