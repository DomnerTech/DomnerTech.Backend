using Bas24.CommandQuery;
using DomnerTech.Backend.Application.Constants;
using DomnerTech.Backend.Application.DTOs;
using DomnerTech.Backend.Application.Errors;
using DomnerTech.Backend.Application.Extensions;
using DomnerTech.Backend.Application.IRepo;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;

namespace DomnerTech.Backend.Application.Features.Notifications.Handlers;

public sealed class GetUnreadCountQueryHandler(
    ILogger<GetUnreadCountQueryHandler> logger,
    INotificationRepo notificationRepo,
    IHttpContextAccessor httpContextAccessor) : IRequestHandler<GetUnreadCountQuery, BaseResponse<int>>
{
    public async Task<BaseResponse<int>> Handle(GetUnreadCountQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var employeeId = httpContextAccessor.HttpContext?.GetClaim(ClaimConstant.EmpId).ToObjectId() ?? ObjectId.Empty;

            var count = await notificationRepo.GetUnreadCountAsync(employeeId, cancellationToken);

            return new BaseResponse<int> { Data = count };
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error getting unread count: {Error}", e.Message);
        }

        return new BaseResponse<int>
        {
            Data = 0,
            Status = new ResponseStatus
            {
                StatusCode = StatusCodes.Status500InternalServerError,
                ErrorCode = ErrorCodes.SystemError
            }
        };
    }
}