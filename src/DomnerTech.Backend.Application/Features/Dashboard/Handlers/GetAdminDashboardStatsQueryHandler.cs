using Bas24.CommandQuery;
using DomnerTech.Backend.Application.DTOs;
using DomnerTech.Backend.Application.DTOs.Leaves.Dashboard;
using DomnerTech.Backend.Application.Errors;
using DomnerTech.Backend.Application.IRepo;
using DomnerTech.Backend.Domain.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace DomnerTech.Backend.Application.Features.Dashboard.Handlers;

public sealed class GetAdminDashboardStatsQueryHandler(
    ILogger<GetAdminDashboardStatsQueryHandler> logger,
    ILeaveRequestRepo leaveRequestRepo) : IRequestHandler<GetAdminDashboardStatsQuery, BaseResponse<AdminDashboardStatsDto>>
{
    public async Task<BaseResponse<AdminDashboardStatsDto>> Handle(GetAdminDashboardStatsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var today = DateTime.UtcNow.Date;
            var startOfMonth = new DateTime(today.Year, today.Month, 1, 0, 0, 0, DateTimeKind.Utc);
            var endOfMonth = startOfMonth.AddMonths(1).AddDays(-1);

            var approvedRequests = await leaveRequestRepo.GetByStatusAsync(LeaveRequestStatus.Approved, cancellationToken);
            var pendingRequests = await leaveRequestRepo.GetByStatusAsync(LeaveRequestStatus.Pending, cancellationToken);

            var employeesOnLeaveToday = approvedRequests
                .Count(r => r.Period.StartDate.Date <= today && r.Period.EndDate.Date >= today);

            var upcomingLeaves = approvedRequests
                .Count(r => r.Period.StartDate.Date > today && r.Period.StartDate.Date <= today.AddDays(30));

            var thisMonthRequests = approvedRequests
                .Where(r => r.Period.StartDate >= startOfMonth && r.Period.StartDate <= endOfMonth)
                .ToList();

            var totalLeaveDaysThisMonth = thisMonthRequests.Sum(r => r.TotalDays);

            var stats = new AdminDashboardStatsDto
            {
                TotalEmployees = 0, // Would query employee count
                EmployeesOnLeaveToday = employeesOnLeaveToday,
                PendingApprovals = pendingRequests.Count,
                UpcomingLeaves = upcomingLeaves,
                TotalLeaveDaysThisMonth = totalLeaveDaysThisMonth,
                AverageLeavePerEmployee = 0, // Would calculate based on employee count
                TotalRequestsThisMonth = thisMonthRequests.Count,
                ApprovedThisMonth = thisMonthRequests.Count,
                RejectedThisMonth = 0
            };

            return new BaseResponse<AdminDashboardStatsDto> { Data = stats };
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error getting dashboard stats: {Error}", e.Message);
        }

        return new BaseResponse<AdminDashboardStatsDto>
        {
            Data = null!,
            Status = new ResponseStatus
            {
                StatusCode = StatusCodes.Status500InternalServerError,
                ErrorCode = ErrorCodes.SystemError
            }
        };
    }
}