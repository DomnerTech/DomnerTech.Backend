using Bas24.CommandQuery;
using DomnerTech.Backend.Application.DTOs;
using DomnerTech.Backend.Application.Errors;
using DomnerTech.Backend.Application.IRepo;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;

namespace DomnerTech.Backend.Application.Features.Notifications.Handlers;

public sealed class MarkNotificationAsReadCommandHandler(
    ILogger<MarkNotificationAsReadCommandHandler> logger,
    INotificationRepo notificationRepo) : IRequestHandler<MarkNotificationAsReadCommand, BaseResponse<bool>>
{
    public async Task<BaseResponse<bool>> Handle(MarkNotificationAsReadCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var notificationId = ObjectId.Parse(request.NotificationId);
            await notificationRepo.MarkAsReadAsync(notificationId, cancellationToken);

            return new BaseResponse<bool> { Data = true };
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error marking notification as read: {Error}", e.Message);
        }

        return new BaseResponse<bool>
        {
            Data = false,
            Status = new ResponseStatus
            {
                StatusCode = StatusCodes.Status500InternalServerError,
                ErrorCode = ErrorCodes.SystemError
            }
        };
    }
}