using Bas24.CommandQuery;
using DomnerTech.Backend.Application.Abstractions;
using DomnerTech.Backend.Application.DTOs;
using DomnerTech.Backend.Application.DTOs.Leaves.Dashboard;

namespace DomnerTech.Backend.Application.Features.Dashboard;

/// <summary>
/// Query to get upcoming leaves (next 30 days).
/// </summary>
public sealed record GetUpcomingLeavesQuery(int Days = 30) :
    IRequest<BaseResponse<List<UpcomingLeaveDto>>>,
    ILogCreator;