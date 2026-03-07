using Bas24.CommandQuery;
using DomnerTech.Backend.Application.Constants;
using DomnerTech.Backend.Application.DTOs;
using DomnerTech.Backend.Application.DTOs.Notifications;
using DomnerTech.Backend.Application.Errors;
using DomnerTech.Backend.Application.Extensions;
using DomnerTech.Backend.Application.IRepo;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;

namespace DomnerTech.Backend.Application.Features.Notifications.Handlers;

public sealed class GetUnreadNotificationsQueryHandler(
    ILogger<GetUnreadNotificationsQueryHandler> logger,
    INotificationRepo notificationRepo,
    IHttpContextAccessor httpContextAccessor) : IRequestHandler<GetUnreadNotificationsQuery, BaseResponse<IEnumerable<NotificationDto>>>
{
    public async Task<BaseResponse<IEnumerable<NotificationDto>>> Handle(GetUnreadNotificationsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var employeeId = httpContextAccessor.HttpContext?.GetClaim(ClaimConstant.EmpId).ToObjectId() ?? ObjectId.Empty;

            var entities = await notificationRepo.GetUnreadByRecipientAsync(employeeId, cancellationToken);
            return new BaseResponse<IEnumerable<NotificationDto>>
            {
                Data = entities.Select(e => e.ToDto())
            };
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error getting unread notifications: {Error}", e.Message);
        }

        return new BaseResponse<IEnumerable<NotificationDto>>
        {
            Data = [],
            Status = new ResponseStatus
            {
                StatusCode = StatusCodes.Status500InternalServerError,
                ErrorCode = ErrorCodes.SystemError
            }
        };
    }
}