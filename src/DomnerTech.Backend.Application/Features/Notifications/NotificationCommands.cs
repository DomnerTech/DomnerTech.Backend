using Bas24.CommandQuery;
using DomnerTech.Backend.Application.Abstractions;
using DomnerTech.Backend.Application.DTOs;
using DomnerTech.Backend.Application.DTOs.Notifications;

namespace DomnerTech.Backend.Application.Features.Notifications;

public sealed record GetMyNotificationsQuery(int Limit = 50) :
    IRequest<BaseResponse<List<NotificationDto>>>,
    ILogCreator;

public sealed record GetUnreadNotificationsQuery :
    IRequest<BaseResponse<List<NotificationDto>>>,
    ILogCreator;

public sealed record GetUnreadCountQuery :
    IRequest<BaseResponse<int>>,
    ILogCreator;

public sealed record MarkNotificationAsReadCommand(string NotificationId) :
    IRequest<BaseResponse<bool>>,
    ILogCreator,
    IValidatableRequest;

public sealed record MarkAllAsReadCommand :
    IRequest<BaseResponse<bool>>,
    ILogCreator;
