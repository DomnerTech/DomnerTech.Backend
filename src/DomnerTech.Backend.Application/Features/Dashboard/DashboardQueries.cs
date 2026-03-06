using Bas24.CommandQuery;
using DomnerTech.Backend.Application.Abstractions;
using DomnerTech.Backend.Application.DTOs;
using DomnerTech.Backend.Application.DTOs.Leaves.Dashboard;

namespace DomnerTech.Backend.Application.Features.Dashboard;

/// <summary>
/// Query to get admin dashboard statistics.
/// </summary>
public sealed record GetAdminDashboardStatsQuery :
    IRequest<BaseResponse<AdminDashboardStatsDto>>,
    ILogCreator;

/// <summary>
/// Query to get employees currently on leave.
/// </summary>
public sealed record GetEmployeesOnLeaveQuery :
    IRequest<BaseResponse<List<EmployeeOnLeaveDto>>>,
    ILogCreator;

/// <summary>
/// Query to get upcoming leaves (next 30 days).
/// </summary>
public sealed record GetUpcomingLeavesQuery(int Days = 30) :
    IRequest<BaseResponse<List<UpcomingLeaveDto>>>,
    ILogCreator;

/// <summary>
/// Query to get pending approvals summary.
/// </summary>
public sealed record GetPendingApprovalsSummaryQuery :
    IRequest<BaseResponse<List<PendingApprovalSummaryDto>>>,
    ILogCreator;
