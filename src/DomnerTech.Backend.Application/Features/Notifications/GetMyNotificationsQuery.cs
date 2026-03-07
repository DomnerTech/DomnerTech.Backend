using Bas24.CommandQuery;
using DomnerTech.Backend.Application.Abstractions;
using DomnerTech.Backend.Application.DTOs;
using DomnerTech.Backend.Application.DTOs.Notifications;

namespace DomnerTech.Backend.Application.Features.Notifications;

public sealed record GetMyNotificationsQuery(int Limit = 50) :
    IRequest<BaseResponse<IEnumerable<NotificationDto>>>,
    ILogCreator;