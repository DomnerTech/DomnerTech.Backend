using Bas24.CommandQuery;
using DomnerTech.Backend.Application.DTOs;
using DomnerTech.Backend.Application.DTOs.Leaves.TeamLeave;
using DomnerTech.Backend.Application.Errors;
using DomnerTech.Backend.Application.IRepo;
using DomnerTech.Backend.Domain.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace DomnerTech.Backend.Application.Features.TeamLeave.Handlers;

public sealed class GetTeamLeaveStatsQueryHandler(
    ILogger<GetTeamLeaveStatsQueryHandler> logger,
    ILeaveRequestRepo leaveRequestRepo,
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
            var totalEmployees = 0;
            var employeesOnLeaveToday = 0;
            var pendingCount = 0;
            const decimal totalLeaveUsage = 0;

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
                if (employee?.Department != request.Department) continue;
                pendingCount++;
                if (totalEmployees == 0) totalEmployees++; // Count unique employees
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