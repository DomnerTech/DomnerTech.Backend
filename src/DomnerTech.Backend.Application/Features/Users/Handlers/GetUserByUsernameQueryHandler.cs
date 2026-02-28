using Bas24.CommandQuery;
using DomnerTech.Backend.Application.DTOs;
using DomnerTech.Backend.Application.DTOs.Users;
using DomnerTech.Backend.Application.Errors;
using DomnerTech.Backend.Application.IRepo;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;

namespace DomnerTech.Backend.Application.Features.Users.Handlers;

public sealed class GetUserByUsernameQueryHandler(
    IRedisCache redisCache, 
    IUserRepo userRepo) : IRequestHandler<GetUserByUsernameQuery, BaseResponse<UserDto?>>
{
    public async Task<BaseResponse<UserDto?>> Handle(GetUserByUsernameQuery request, CancellationToken cancellationToken)
    {
        var cacheKey = $":users:username:{request.Username}";
        var userCache = await redisCache.GetObjectAsync<UserDto>(cacheKey, cancellationToken);
        if (userCache is not null)
        {
            return new BaseResponse<UserDto?>
            {
                Data = userCache
            };
        }

        var user = await userRepo.GetByUsernameAsync(request.Username, cancellationToken);
        var userDto = user?.ToDto();
        if (userDto is null)
        {
            return new BaseResponse<UserDto?>
            {
                Status = new ResponseStatus
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    ErrorCode = ErrorCodes.NotFound,
                    Desc = "User not found"
                }
            };
        }

        await redisCache.SetObjectAsync(cacheKey, userDto, new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(360)
        }, cancellationToken);

        return new BaseResponse<UserDto?>
        {
            Data = userDto
        };

    }
}