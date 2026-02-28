using Bas24.CommandQuery;
using DomnerTech.Backend.Application.Abstractions;
using DomnerTech.Backend.Application.DTOs;

namespace DomnerTech.Backend.Application.Features.Roles;

public sealed record UpsertUserRoleCommand(string UserId, string RoleName) : IRequest<BaseResponse<bool>>, ILogReqCreator, IValidatableRequest;