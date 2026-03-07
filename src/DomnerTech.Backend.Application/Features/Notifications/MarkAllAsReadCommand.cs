using Bas24.CommandQuery;
using DomnerTech.Backend.Application.Abstractions;
using DomnerTech.Backend.Application.DTOs;

namespace DomnerTech.Backend.Application.Features.Notifications;

public sealed record MarkAllAsReadCommand :
    IRequest<BaseResponse<bool>>,
    ILogCreator;
