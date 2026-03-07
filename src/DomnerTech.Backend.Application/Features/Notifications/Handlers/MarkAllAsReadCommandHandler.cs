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

public sealed class MarkAllAsReadCommandHandler(
    ILogger<MarkAllAsReadCommandHandler> logger,
    INotificationRepo notificationRepo,
    IHttpContextAccessor httpContextAccessor) : IRequestHandler<MarkAllAsReadCommand, BaseResponse<bool>>
{
    public async Task<BaseResponse<bool>> Handle(MarkAllAsReadCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var employeeId = httpContextAccessor.HttpContext?.GetClaim(ClaimConstant.EmpId).ToObjectId() ?? ObjectId.Empty;

            await notificationRepo.MarkAllAsReadAsync(employeeId, cancellationToken);

            return new BaseResponse<bool> { Data = true };
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error marking all as read: {Error}", e.Message);
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