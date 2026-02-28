using Bas24.CommandQuery;
using DomnerTech.Backend.Application.DTOs;
using DomnerTech.Backend.Application.DTOs.Roles;
using DomnerTech.Backend.Application.Features.Users;

namespace DomnerTech.Backend.Application.Features.Roles.Handlers;

public sealed class GetUserRolesQueryHandler(ICommandQuery commandQuery) : IRequestHandler<GetUserRolesQuery, BaseResponse<IEnumerable<UserRoleDto>>>
{
    public async Task<BaseResponse<IEnumerable<UserRoleDto>>> Handle(GetUserRolesQuery request, CancellationToken cancellationToken)
    {
        var user = await commandQuery.Send(new GetUserQuery(request.UserId), cancellationToken);

        if (!user.IsSuccess)
        {
            return new BaseResponse<IEnumerable<UserRoleDto>>
            {
                Status = user.Status,
                Data = []
            };
        }

        var roles = await commandQuery.Send(new GetAllRolesQuery(), cancellationToken);
        if (!roles.IsSuccess)
        {
            return new BaseResponse<IEnumerable<UserRoleDto>>
            {
                Status = roles.Status,
                Data = []
            };
        }

        var userRoles = roles.Data.Select(role => new UserRoleDto(
            UserId: request.UserId, 
            RoleId: role.Id,
            RoleName: role.Name,
            HasRole: user.Data.Roles.Contains(role.Name)));
        return new BaseResponse<IEnumerable<UserRoleDto>>
        {
            Data = userRoles
        };
    }
}