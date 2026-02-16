using Bas24.CommandQuery;
using DomnerTech.Backend.Application.Abstractions;
using DomnerTech.Backend.Application.DTOs;

namespace DomnerTech.Backend.Application.Features.Roles;

public sealed record CreateRoleCommand(string Name, string? Desc) : 
    IRequest<BaseResponse<bool>>,
    ILogCreator,
    IValidatableRequest;