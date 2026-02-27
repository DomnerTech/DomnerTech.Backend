using Bas24.CommandQuery;
using DomnerTech.Backend.Application.DTOs;
using DomnerTech.Backend.Application.DTOs.Users;
using DomnerTech.Backend.Application.Exceptions;
using DomnerTech.Backend.Application.Extensions;
using DomnerTech.Backend.Application.IRepo;
using Microsoft.Extensions.Caching.Distributed;

namespace DomnerTech.Backend.Application.Features.Users.Handlers;

public sealed class GetUserQueryHandler(IRedisCache redisCache, IUserRepo userRepo) : IRequestHandler<GetUserQuery, BaseResponse<UserDto>>
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
        var user = await userRepo.GetByIdAsync(request.UserId.ToObjectId(), cancellationToken);
        var userDto = user?.ToDto();
        if (userDto == null) throw new NotFoundException($"User not found {request.UserId}");

        await redisCache.SetObjectAsync(cacheKey, userDto, new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(360)
        }, cancellationToken);
        return new BaseResponse<UserDto>
        {
            Data = userDto
        };

    }
}