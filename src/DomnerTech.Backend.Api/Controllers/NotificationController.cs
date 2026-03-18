using Bas24.CommandQuery;
using DomnerTech.Backend.Application.DTOs;
using DomnerTech.Backend.Application.DTOs.Notifications;
using DomnerTech.Backend.Application.Features.Notifications;
using Microsoft.AspNetCore.Mvc;

namespace DomnerTech.Backend.Api.Controllers;

/// <summary>
/// Controller for managing notifications.
/// </summary>
public sealed class NotificationController(ICommandQuery commandQuery) : BaseApiController(commandQuery)
{
    /// <summary>
    /// Gets the current user's notifications.
    /// </summary>
    /// <param name="limit">Maximum number of notifications to return (default: 50).</param>
    [HttpGet]
    public async Task<ActionResult<BaseResponse<List<NotificationDto>>>> GetMyNotifications([FromQuery] int limit = 50)
    {
        var result = await _commandQuery.Send(new GetMyNotificationsQuery(limit), HttpContext.RequestAborted);
        return await ReturnJson(result);
    }

    /// <summary>
    /// Gets unread notifications for the current user.
    /// </summary>
    [HttpGet("unread")]
    public async Task<ActionResult<BaseResponse<List<NotificationDto>>>> GetUnreadNotifications()
    {
        var result = await _commandQuery.Send(new GetUnreadNotificationsQuery(), HttpContext.RequestAborted);
        return await ReturnJson(result);
    }

    /// <summary>
    /// Gets the count of unread notifications.
    /// </summary>
    [HttpGet("unread/count")]
    public async Task<ActionResult<BaseResponse<int>>> GetUnreadCount()
    {
        var result = await _commandQuery.Send(new GetUnreadCountQuery(), HttpContext.RequestAborted);
        return await ReturnJson(result);
    }

    /// <summary>
    /// Marks a notification as read.
    /// </summary>
    [HttpPut("{id}/read")]
    public async Task<ActionResult<BaseResponse<bool>>> MarkAsRead([FromRoute] string id)
    {
        var result = await _commandQuery.Send(new MarkNotificationAsReadCommand(id), HttpContext.RequestAborted);
        return await ReturnJson(result);
    }

    /// <summary>
    /// Marks all notifications as read.
    /// </summary>
    [HttpPut("read-all")]
    public async Task<ActionResult<BaseResponse<bool>>> MarkAllAsRead()
    {
        var result = await _commandQuery.Send(new MarkAllAsReadCommand(), HttpContext.RequestAborted);
        return await ReturnJson(result);
    }
}
