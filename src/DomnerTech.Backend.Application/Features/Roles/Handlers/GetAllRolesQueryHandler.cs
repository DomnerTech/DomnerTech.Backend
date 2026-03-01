using Bas24.CommandQuery;
using DomnerTech.Backend.Application.Caching;
using DomnerTech.Backend.Application.DTOs;
using DomnerTech.Backend.Application.DTOs.Roles;
using DomnerTech.Backend.Application.IRepo;

namespace DomnerTech.Backend.Application.Features.Roles.Handlers;

public sealed class GetAllRolesQueryHandler(IRoleRepo roleRepo, IRedisCache redisCache) : IRequestHandler<GetAllRolesQuery, BaseResponse<IEnumerable<RoleDto>>>
{
    public async Task<BaseResponse<IEnumerable<RoleDto>>> Handle(GetAllRolesQuery request, CancellationToken cancellationToken)
    {
        const string cacheKey = ":roles";
        var rolesCache = await redisCache.GetObjectAsync<IEnumerable<RoleDto>>(cacheKey);
        if (rolesCache is not null)
        {
            return new BaseResponse<IEnumerable<RoleDto>>
            {
                Data = rolesCache
            };
        }

        var roles = await roleRepo.GetAllAsync(cancellationToken);
        var roleDto = roles.Select(i => i.ToDto()).ToList();
        await redisCache.SetObjectAsync(cacheKey, roleDto, new CacheEntryOptions
        {
            AbsoluteExpiration = DateTime.UtcNow.AddDays(7)
        });
        return new BaseResponse<IEnumerable<RoleDto>>
        {
            Data = roleDto
        };
    }
}