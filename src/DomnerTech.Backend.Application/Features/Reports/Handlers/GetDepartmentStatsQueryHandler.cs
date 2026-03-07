using Bas24.CommandQuery;
using DomnerTech.Backend.Application.DTOs;
using DomnerTech.Backend.Application.DTOs.Leaves.Reports;
using DomnerTech.Backend.Application.Errors;
using DomnerTech.Backend.Application.IRepo;
using DomnerTech.Backend.Domain.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace DomnerTech.Backend.Application.Features.Reports.Handlers;

/// <summary>
/// Handler for GetDepartmentStatsQuery.
/// </summary>
public sealed class GetDepartmentStatsQueryHandler(
    ILogger<GetDepartmentStatsQueryHandler> logger,
    ILeaveRequestRepo leaveRequestRepo,
    IEmployeeRepo employeeRepo) : IRequestHandler<GetDepartmentStatsQuery, BaseResponse<List<DepartmentLeaveStatsDto>>>
{
    public async Task<BaseResponse<List<DepartmentLeaveStatsDto>>> Handle(GetDepartmentStatsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var allRequests = await leaveRequestRepo.GetByStatusAsync(LeaveRequestStatus.Approved, cancellationToken);
            var pendingRequests = await leaveRequestRepo.GetByStatusAsync(LeaveRequestStatus.Pending, cancellationToken);
            var rejectedRequests = await leaveRequestRepo.GetByStatusAsync(LeaveRequestStatus.Rejected, cancellationToken);

            // Group requests by department
            var departmentGroups = new Dictionary<string, List<(string EmployeeId, decimal Days, LeaveRequestStatus Status)>>();

            // Process all request types
            var allRequestsList = new[] { 
                (allRequests, LeaveRequestStatus.Approved),
                (pendingRequests, LeaveRequestStatus.Pending),
                (rejectedRequests, LeaveRequestStatus.Rejected)
            };

            foreach (var (requests, status) in allRequestsList)
            {
                foreach (var leaveRequest in requests)
                {
                    var employee = await employeeRepo.GetByIdAsync(leaveRequest.EmployeeId, cancellationToken);
                    if (employee == null) continue;

                    // Filter by department if specified
                    if (!string.IsNullOrWhiteSpace(request.Department) && 
                        !employee.Department.Equals(request.Department, StringComparison.OrdinalIgnoreCase))
                    {
                        continue;
                    }

                    if (!departmentGroups.TryGetValue(employee.Department, out var value))
                    {
                        value = [];
                        departmentGroups[employee.Department] = value;
                    }

                    value.Add((
                        employee.Id.ToString(),
                        leaveRequest.Period.TotalDays,
                        status
                    ));
                }
            }

            var stats = new List<DepartmentLeaveStatsDto>();

            foreach (var (department, requests) in departmentGroups)
            {
                var uniqueEmployees = requests.Select(r => r.EmployeeId).Distinct().Count();
                var totalLeaveDays = requests.Sum(r => r.Days);
                var averageLeaveDays = uniqueEmployees > 0 ? totalLeaveDays / uniqueEmployees : 0;

                stats.Add(new DepartmentLeaveStatsDto
                {
                    Department = department,
                    TotalEmployees = uniqueEmployees,
                    TotalLeaveDays = totalLeaveDays,
                    AverageLeaveDays = Math.Round(averageLeaveDays, 2),
                    TotalRequests = requests.Count,
                    ApprovedRequests = requests.Count(r => r.Status == LeaveRequestStatus.Approved),
                    PendingRequests = requests.Count(r => r.Status == LeaveRequestStatus.Pending),
                    RejectedRequests = requests.Count(r => r.Status == LeaveRequestStatus.Rejected)
                });
            }

            return new BaseResponse<List<DepartmentLeaveStatsDto>> { Data = stats };
        }
        catch (OperationCanceledException)
        {
            throw;
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error getting department statistics: {Error}", e.Message);
        }

        return new BaseResponse<List<DepartmentLeaveStatsDto>>
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
