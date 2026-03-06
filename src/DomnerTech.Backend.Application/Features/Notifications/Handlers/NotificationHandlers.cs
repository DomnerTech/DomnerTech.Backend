using Bas24.CommandQuery;
using DomnerTech.Backend.Application.DTOs;
using DomnerTech.Backend.Application.DTOs.Notifications;
using DomnerTech.Backend.Application.Errors;
using DomnerTech.Backend.Application.Extensions;
using DomnerTech.Backend.Application.IRepo;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;

namespace DomnerTech.Backend.Application.Features.Notifications.Handlers;

public sealed class GetMyNotificationsQueryHandler(
    ILogger<GetMyNotificationsQueryHandler> logger,
    INotificationRepo notificationRepo,
    IHttpContextAccessor httpContextAccessor) : IRequestHandler<GetMyNotificationsQuery, BaseResponse<List<NotificationDto>>>
{
    public async Task<BaseResponse<List<NotificationDto>>> Handle(GetMyNotificationsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var employeeId = httpContextAccessor.HttpContext?.User.Claims
                .FirstOrDefault(c => c.Type == "EmployeeId")?.Value.ToObjectId() ?? ObjectId.Empty;

            var entities = await notificationRepo.GetByRecipientAsync(employeeId, request.Limit, cancellationToken);

            var dtos = entities.Select(e => new NotificationDto
            {
                Id = e.Id.ToString(),
                Type = e.Type,
                Title = e.Title,
                Message = e.Message,
                RelatedEntityId = e.RelatedEntityId?.ToString(),
                IsRead = e.IsRead,
                ReadAt = e.ReadAt,
                Priority = e.Priority,
                CreatedAt = e.CreatedAt
            }).ToList();

            return new BaseResponse<List<NotificationDto>> { Data = dtos };
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error getting notifications: {Error}", e.Message);
        }

        return new BaseResponse<List<NotificationDto>>
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

public sealed class GetUnreadNotificationsQueryHandler(
    ILogger<GetUnreadNotificationsQueryHandler> logger,
    INotificationRepo notificationRepo,
    IHttpContextAccessor httpContextAccessor) : IRequestHandler<GetUnreadNotificationsQuery, BaseResponse<List<NotificationDto>>>
{
    public async Task<BaseResponse<List<NotificationDto>>> Handle(GetUnreadNotificationsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var employeeId = httpContextAccessor.HttpContext?.User.Claims
                .FirstOrDefault(c => c.Type == "EmployeeId")?.Value.ToObjectId() ?? ObjectId.Empty;

            var entities = await notificationRepo.GetUnreadByRecipientAsync(employeeId, cancellationToken);

            var dtos = entities.Select(e => new NotificationDto
            {
                Id = e.Id.ToString(),
                Type = e.Type,
                Title = e.Title,
                Message = e.Message,
                RelatedEntityId = e.RelatedEntityId?.ToString(),
                IsRead = e.IsRead,
                ReadAt = e.ReadAt,
                Priority = e.Priority,
                CreatedAt = e.CreatedAt
            }).ToList();

            return new BaseResponse<List<NotificationDto>> { Data = dtos };
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error getting unread notifications: {Error}", e.Message);
        }

        return new BaseResponse<List<NotificationDto>>
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

public sealed class GetUnreadCountQueryHandler(
    ILogger<GetUnreadCountQueryHandler> logger,
    INotificationRepo notificationRepo,
    IHttpContextAccessor httpContextAccessor) : IRequestHandler<GetUnreadCountQuery, BaseResponse<int>>
{
    public async Task<BaseResponse<int>> Handle(GetUnreadCountQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var employeeId = httpContextAccessor.HttpContext?.User.Claims
                .FirstOrDefault(c => c.Type == "EmployeeId")?.Value.ToObjectId() ?? ObjectId.Empty;

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

public sealed class MarkAllAsReadCommandHandler(
    ILogger<MarkAllAsReadCommandHandler> logger,
    INotificationRepo notificationRepo,
    IHttpContextAccessor httpContextAccessor) : IRequestHandler<MarkAllAsReadCommand, BaseResponse<bool>>
{
    public async Task<BaseResponse<bool>> Handle(MarkAllAsReadCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var employeeId = httpContextAccessor.HttpContext?.User.Claims
                .FirstOrDefault(c => c.Type == "EmployeeId")?.Value.ToObjectId() ?? ObjectId.Empty;

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
