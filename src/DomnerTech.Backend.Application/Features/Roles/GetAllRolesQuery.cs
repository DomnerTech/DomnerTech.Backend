using Bas24.CommandQuery;
using DomnerTech.Backend.Application.Abstractions;
using DomnerTech.Backend.Application.DTOs;
using DomnerTech.Backend.Application.DTOs.Roles;

namespace DomnerTech.Backend.Application.Features.Roles;

public record GetAllRolesQuery : IRequest<BaseResponse<IEnumerable<RoleDto>>>, ILogReqCreator;