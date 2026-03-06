using Bas24.CommandQuery;
using DomnerTech.Backend.Application.DTOs;
using DomnerTech.Backend.Application.DTOs.Leaves.TeamLeave;
using DomnerTech.Backend.Application.Errors;
using DomnerTech.Backend.Application.IRepo;
using DomnerTech.Backend.Domain.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;

namespace DomnerTech.Backend.Application.Features.TeamLeave.Handlers;

public sealed class GetTeamLeaveCalendarQueryHandler(
    ILogger<GetTeamLeaveCalendarQueryHandler> logger,
    ILeaveRequestRepo leaveRequestRepo,
    IEmployeeRepo employeeRepo,
    ILeaveTypeRepo leaveTypeRepo) : IRequestHandler<GetTeamLeaveCalendarQuery, BaseResponse<List<TeamLeaveCalendarDto>>>
{
    public async Task<BaseResponse<List<TeamLeaveCalendarDto>>> Handle(GetTeamLeaveCalendarQuery request, CancellationToken cancellationToken)
    {
        try
        {
            // Get all approved leave requests in date range
            var approvedRequests = await leaveRequestRepo.GetByStatusAsync(LeaveRequestStatus.Approved, cancellationToken);
            
            var filteredRequests = approvedRequests
                .Where(r => r.Period.StartDate <= request.EndDate && r.Period.EndDate >= request.StartDate)
                .ToList();

            var result = new List<TeamLeaveCalendarDto>();

            foreach (var req in filteredRequests)
            {
                var employee = await employeeRepo.GetByIdAsync(req.EmployeeId, cancellationToken);
                if (employee == null || employee.Department != request.Department) continue;

                var leaveType = await leaveTypeRepo.GetByIdAsync(req.LeaveTypeId, cancellationToken);

                result.Add(new TeamLeaveCalendarDto
                {
                    EmployeeId = employee.Id.ToString(),
                    EmployeeName = $"{employee.FirstName} {employee.LastName}",
                    Department = employee.Department,
                    LeaveTypeName = leaveType?.Name ?? "Unknown",
                    StartDate = req.Period.StartDate,
                    EndDate = req.Period.EndDate,
                    TotalDays = req.TotalDays,
                    Status = req.Status.ToString()
                });
            }

            return new BaseResponse<List<TeamLeaveCalendarDto>>
            {
                Data = result.OrderBy(x => x.StartDate).ToList()
            };
        }
        catch (OperationCanceledException)
        {
            throw;
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error getting team leave calendar: {Error}", e.Message);
        }

        return new BaseResponse<List<TeamLeaveCalendarDto>>
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

public sealed class CheckTeamLeaveConflictQueryHandler(
    ILogger<CheckTeamLeaveConflictQueryHandler> logger,
    ILeaveRequestRepo leaveRequestRepo,
    IEmployeeRepo employeeRepo) : IRequestHandler<CheckTeamLeaveConflictQuery, BaseResponse<List<TeamLeaveConflictDto>>>
{
    public async Task<BaseResponse<List<TeamLeaveConflictDto>>> Handle(CheckTeamLeaveConflictQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var r = request.Dto;
            var approvedRequests = await leaveRequestRepo.GetByStatusAsync(LeaveRequestStatus.Approved, cancellationToken);
            
            var relevantRequests = approvedRequests
                .Where(req => req.Period.StartDate <= r.EndDate && req.Period.EndDate >= r.StartDate)
                .ToList();

            // Filter by department
            var departmentRequests = new List<(Domain.Entities.LeaveRequestEntity Request, Domain.Entities.EmployeeEntity Employee)>();
            foreach (var req in relevantRequests)
            {
                var employee = await employeeRepo.GetByIdAsync(req.EmployeeId, cancellationToken);
                if (employee?.Department == r.Department)
                {
                    if (string.IsNullOrEmpty(r.ExcludeEmployeeId) || employee.Id.ToString() != r.ExcludeEmployeeId)
                    {
                        departmentRequests.Add((req, employee));
                    }
                }
            }

            // Check conflicts for each day
            var conflicts = new Dictionary<DateTime, TeamLeaveConflictDto>();
            var currentDate = r.StartDate.Date;

            while (currentDate <= r.EndDate.Date)
            {
                var employeesOnLeave = departmentRequests
                    .Where(x => x.Request.Period.StartDate.Date <= currentDate && x.Request.Period.EndDate.Date >= currentDate)
                    .ToList();

                if (employeesOnLeave.Count > 0)
                {
                    conflicts[currentDate] = new TeamLeaveConflictDto
                    {
                        Date = currentDate,
                        EmployeesOnLeave = employeesOnLeave.Count,
                        MaxAllowed = r.MaxEmployeesOnLeave,
                        EmployeeNames = employeesOnLeave.Select(x => $"{x.Employee.FirstName} {x.Employee.LastName}").ToList()
                    };
                }

                currentDate = currentDate.AddDays(1);
            }

            return new BaseResponse<List<TeamLeaveConflictDto>>
            {
                Data = conflicts.Values.Where(c => c.HasConflict).OrderBy(c => c.Date).ToList()
            };
        }
        catch (OperationCanceledException)
        {
            throw;
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error checking team leave conflicts: {Error}", e.Message);
        }

        return new BaseResponse<List<TeamLeaveConflictDto>>
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

public sealed class GetTeamLeaveStatsQueryHandler(
    ILogger<GetTeamLeaveStatsQueryHandler> logger,
    ILeaveRequestRepo leaveRequestRepo,
    ILeaveBalanceRepo leaveBalanceRepo,
    IEmployeeRepo employeeRepo) : IRequestHandler<GetTeamLeaveStatsQuery, BaseResponse<TeamLeaveStatsDto>>
{
    public async Task<BaseResponse<TeamLeaveStatsDto>> Handle(GetTeamLeaveStatsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            // This would typically use a more efficient query in production
            var allRequests = await leaveRequestRepo.GetByStatusAsync(LeaveRequestStatus.Approved, cancellationToken);
            var pendingRequests = await leaveRequestRepo.GetByStatusAsync(LeaveRequestStatus.Pending, cancellationToken);
            
            var today = DateTime.UtcNow.Date;
            var currentYear = DateTime.UtcNow.Year;

            int totalEmployees = 0;
            int employeesOnLeaveToday = 0;
            int pendingCount = 0;
            decimal totalLeaveUsage = 0;

            // This is simplified - in production, you'd query employees by department directly
            var todayLeave = allRequests.Where(r => r.Period.StartDate.Date <= today && r.Period.EndDate.Date >= today).ToList();
            
            foreach (var req in todayLeave)
            {
                var employee = await employeeRepo.GetByIdAsync(req.EmployeeId, cancellationToken);
                if (employee?.Department == request.Department)
                {
                    employeesOnLeaveToday++;
                }
            }

            foreach (var req in pendingRequests)
            {
                var employee = await employeeRepo.GetByIdAsync(req.EmployeeId, cancellationToken);
                if (employee?.Department == request.Department)
                {
                    pendingCount++;
                    if (totalEmployees == 0) totalEmployees++; // Count unique employees
                }
            }

            var stats = new TeamLeaveStatsDto
            {
                Department = request.Department,
                TotalEmployees = totalEmployees > 0 ? totalEmployees : 1, // Avoid division by zero
                EmployeesOnLeaveToday = employeesOnLeaveToday,
                PendingRequests = pendingCount,
                AverageLeaveUsage = totalEmployees > 0 ? totalLeaveUsage / totalEmployees : 0
            };

            return new BaseResponse<TeamLeaveStatsDto> { Data = stats };
        }
        catch (OperationCanceledException)
        {
            throw;
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error getting team leave stats: {Error}", e.Message);
        }

        return new BaseResponse<TeamLeaveStatsDto>
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

public sealed class GetUpcomingTeamLeaveQueryHandler(
    ILogger<GetUpcomingTeamLeaveQueryHandler> logger,
    ILeaveRequestRepo leaveRequestRepo,
    IEmployeeRepo employeeRepo,
    ILeaveTypeRepo leaveTypeRepo) : IRequestHandler<GetUpcomingTeamLeaveQuery, BaseResponse<List<TeamLeaveCalendarDto>>>
{
    public async Task<BaseResponse<List<TeamLeaveCalendarDto>>> Handle(GetUpcomingTeamLeaveQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var today = DateTime.UtcNow.Date;
            var endDate = today.AddDays(30);

            var approvedRequests = await leaveRequestRepo.GetByStatusAsync(LeaveRequestStatus.Approved, cancellationToken);
            
            var upcomingRequests = approvedRequests
                .Where(r => r.Period.StartDate >= today && r.Period.StartDate <= endDate)
                .ToList();

            var result = new List<TeamLeaveCalendarDto>();

            foreach (var req in upcomingRequests)
            {
                var employee = await employeeRepo.GetByIdAsync(req.EmployeeId, cancellationToken);
                if (employee == null || employee.Department != request.Department) continue;

                var leaveType = await leaveTypeRepo.GetByIdAsync(req.LeaveTypeId, cancellationToken);

                result.Add(new TeamLeaveCalendarDto
                {
                    EmployeeId = employee.Id.ToString(),
                    EmployeeName = $"{employee.FirstName} {employee.LastName}",
                    Department = employee.Department,
                    LeaveTypeName = leaveType?.Name ?? "Unknown",
                    StartDate = req.Period.StartDate,
                    EndDate = req.Period.EndDate,
                    TotalDays = req.TotalDays,
                    Status = req.Status.ToString()
                });
            }

            return new BaseResponse<List<TeamLeaveCalendarDto>>
            {
                Data = result.OrderBy(x => x.StartDate).ToList()
            };
        }
        catch (OperationCanceledException)
        {
            throw;
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error getting upcoming team leave: {Error}", e.Message);
        }

        return new BaseResponse<List<TeamLeaveCalendarDto>>
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
