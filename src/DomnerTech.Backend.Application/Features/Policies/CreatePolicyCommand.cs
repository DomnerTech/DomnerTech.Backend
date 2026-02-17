using Bas24.CommandQuery;
using DomnerTech.Backend.Application.Abstractions;
using DomnerTech.Backend.Application.DTOs;

namespace DomnerTech.Backend.Application.Features.Policies;

public sealed record CreatePolicyCommand(string Name, HashSet<string> RoleNames, string? Desc) :
    IRequest<BaseResponse<string>>,
    ILogCreator, IValidatableRequest;