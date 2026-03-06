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
    ILeaveRequestRepo leaveRequestRepo,
    IEmployeeRepo employeeRepo) : IRequestHandler<GetAdminDashboardStatsQuery, BaseResponse<AdminDashboardStatsDto>>
{
    public async Task<BaseResponse<AdminDashboardStatsDto>> Handle(GetAdminDashboardStatsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var today = DateTime.UtcNow.Date;
            var startOfMonth = new DateTime(today.Year, today.Month, 1);
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

public sealed class GetEmployeesOnLeaveQueryHandler(
    ILogger<GetEmployeesOnLeaveQueryHandler> logger,
    ILeaveRequestRepo leaveRequestRepo,
    IEmployeeRepo employeeRepo,
    ILeaveTypeRepo leaveTypeRepo) : IRequestHandler<GetEmployeesOnLeaveQuery, BaseResponse<List<EmployeeOnLeaveDto>>>
{
    public async Task<BaseResponse<List<EmployeeOnLeaveDto>>> Handle(GetEmployeesOnLeaveQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var today = DateTime.UtcNow.Date;
            var approvedRequests = await leaveRequestRepo.GetByStatusAsync(LeaveRequestStatus.Approved, cancellationToken);

            var onLeaveToday = approvedRequests
                .Where(r => r.Period.StartDate.Date <= today && r.Period.EndDate.Date >= today)
                .ToList();

            var result = new List<EmployeeOnLeaveDto>();

            foreach (var req in onLeaveToday)
            {
                var employee = await employeeRepo.GetByIdAsync(req.EmployeeId, cancellationToken);
                var leaveType = await leaveTypeRepo.GetByIdAsync(req.LeaveTypeId, cancellationToken);

                if (employee != null)
                {
                    result.Add(new EmployeeOnLeaveDto
                    {
                        EmployeeId = employee.Id.ToString(),
                        EmployeeName = $"{employee.FirstName} {employee.LastName}",
                        Department = employee.Department,
                        LeaveTypeName = leaveType?.Name ?? "Unknown",
                        StartDate = req.Period.StartDate,
                        EndDate = req.Period.EndDate,
                        TotalDays = req.TotalDays
                    });
                }
            }

            return new BaseResponse<List<EmployeeOnLeaveDto>> { Data = result };
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error getting employees on leave: {Error}", e.Message);
        }

        return new BaseResponse<List<EmployeeOnLeaveDto>>
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

public sealed class GetUpcomingLeavesQueryHandler(
    ILogger<GetUpcomingLeavesQueryHandler> logger,
    ILeaveRequestRepo leaveRequestRepo,
    IEmployeeRepo employeeRepo,
    ILeaveTypeRepo leaveTypeRepo) : IRequestHandler<GetUpcomingLeavesQuery, BaseResponse<List<UpcomingLeaveDto>>>
{
    public async Task<BaseResponse<List<UpcomingLeaveDto>>> Handle(GetUpcomingLeavesQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var today = DateTime.UtcNow.Date;
            var endDate = today.AddDays(request.Days);

            var approvedRequests = await leaveRequestRepo.GetByStatusAsync(LeaveRequestStatus.Approved, cancellationToken);

            var upcomingLeaves = approvedRequests
                .Where(r => r.Period.StartDate.Date > today && r.Period.StartDate.Date <= endDate)
                .OrderBy(r => r.Period.StartDate)
                .ToList();

            var result = new List<UpcomingLeaveDto>();

            foreach (var req in upcomingLeaves)
            {
                var employee = await employeeRepo.GetByIdAsync(req.EmployeeId, cancellationToken);
                var leaveType = await leaveTypeRepo.GetByIdAsync(req.LeaveTypeId, cancellationToken);

                if (employee != null)
                {
                    var daysUntil = (req.Period.StartDate.Date - today).Days;
                    result.Add(new UpcomingLeaveDto
                    {
                        EmployeeId = employee.Id.ToString(),
                        EmployeeName = $"{employee.FirstName} {employee.LastName}",
                        Department = employee.Department,
                        LeaveTypeName = leaveType?.Name ?? "Unknown",
                        StartDate = req.Period.StartDate,
                        EndDate = req.Period.EndDate,
                        TotalDays = req.TotalDays,
                        DaysUntilStart = daysUntil
                    });
                }
            }

            return new BaseResponse<List<UpcomingLeaveDto>> { Data = result };
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error getting upcoming leaves: {Error}", e.Message);
        }

        return new BaseResponse<List<UpcomingLeaveDto>>
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

public sealed class GetPendingApprovalsSummaryQueryHandler(
    ILogger<GetPendingApprovalsSummaryQueryHandler> logger,
    ILeaveRequestRepo leaveRequestRepo,
    IEmployeeRepo employeeRepo,
    ILeaveTypeRepo leaveTypeRepo) : IRequestHandler<GetPendingApprovalsSummaryQuery, BaseResponse<List<PendingApprovalSummaryDto>>>
{
    public async Task<BaseResponse<List<PendingApprovalSummaryDto>>> Handle(GetPendingApprovalsSummaryQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var pendingRequests = await leaveRequestRepo.GetByStatusAsync(LeaveRequestStatus.Pending, cancellationToken);
            var result = new List<PendingApprovalSummaryDto>();

            foreach (var req in pendingRequests.OrderBy(r => r.SubmittedAt))
            {
                var employee = await employeeRepo.GetByIdAsync(req.EmployeeId, cancellationToken);
                var leaveType = await leaveTypeRepo.GetByIdAsync(req.LeaveTypeId, cancellationToken);

                if (employee != null)
                {
                    var pendingDays = (DateTime.UtcNow.Date - req.SubmittedAt.Date).Days;
                    result.Add(new PendingApprovalSummaryDto
                    {
                        LeaveRequestId = req.Id.ToString(),
                        EmployeeId = employee.Id.ToString(),
                        EmployeeName = $"{employee.FirstName} {employee.LastName}",
                        Department = employee.Department,
                        LeaveTypeName = leaveType?.Name ?? "Unknown",
                        StartDate = req.Period.StartDate,
                        EndDate = req.Period.EndDate,
                        TotalDays = req.TotalDays,
                        SubmittedAt = req.SubmittedAt,
                        PendingDays = pendingDays
                    });
                }
            }

            return new BaseResponse<List<PendingApprovalSummaryDto>> { Data = result };
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error getting pending approvals summary: {Error}", e.Message);
        }

        return new BaseResponse<List<PendingApprovalSummaryDto>>
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
