using Bas24.CommandQuery;
using DomnerTech.Backend.Application.DTOs;
using DomnerTech.Backend.Application.DTOs.Users;
using DomnerTech.Backend.Application.IRepo;
using Microsoft.Extensions.Caching.Distributed;

namespace DomnerTech.Backend.Application.Features.Users.Handlers;

public sealed class GetUserQueryHandler(IRedisCache redisCache) : IRequestHandler<GetUserQuery, BaseResponse<UserDto>>
{
    public async Task<BaseResponse<UserDto>> Handle(GetUserQuery request, CancellationToken cancellationToken)
    {
        var cacheKey = $":users:{request.UserId}";
        var userCache = await redisCache.GetObjectAsync<UserDto>(cacheKey, cancellationToken);
        if (userCache != null)
            return new BaseResponse<UserDto>
            {
                Data = userCache
            };
        var user = new UserDto
        {
            Username = "JohnDoe",
            EmpId = null,
            Id = null,
            CompanyId = null
        };
        await redisCache.SetObjectAsync(cacheKey, user, new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(360)
        }, cancellationToken);
        return await Task.FromResult(new BaseResponse<UserDto>
        {
            Data = user
        });
    }
}