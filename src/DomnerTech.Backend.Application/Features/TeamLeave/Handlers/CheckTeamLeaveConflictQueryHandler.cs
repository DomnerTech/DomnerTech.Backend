using Bas24.CommandQuery;
using DomnerTech.Backend.Application.DTOs;
using DomnerTech.Backend.Application.DTOs.Leaves.TeamLeave;
using DomnerTech.Backend.Application.Errors;
using DomnerTech.Backend.Application.IRepo;
using DomnerTech.Backend.Domain.Entities;
using DomnerTech.Backend.Domain.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace DomnerTech.Backend.Application.Features.TeamLeave.Handlers;

public sealed class CheckTeamLeaveConflictQueryHandler(
    ILogger<CheckTeamLeaveConflictQueryHandler> logger,
    ILeaveRequestRepo leaveRequestRepo,
    IEmployeeRepo employeeRepo) : IRequestHandler<CheckTeamLeaveConflictQuery, BaseResponse<IEnumerable<TeamLeaveConflictDto>>>
{
    public async Task<BaseResponse<IEnumerable<TeamLeaveConflictDto>>> Handle(CheckTeamLeaveConflictQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var r = request.Dto;
            var approvedRequests = await leaveRequestRepo.GetByStatusAsync(LeaveRequestStatus.Approved, cancellationToken);
            
            var relevantRequests = approvedRequests
                .Where(req => req.Period.StartDate <= r.EndDate && req.Period.EndDate >= r.StartDate)
                .ToList();

            // Filter by department
            var departmentRequests = new List<(LeaveRequestEntity Request, EmployeeEntity Employee)>();
            foreach (var req in relevantRequests)
            {
                var employee = await employeeRepo.GetByIdAsync(req.EmployeeId, cancellationToken);
                if (employee?.Department != r.Department) continue;
                if (string.IsNullOrEmpty(r.ExcludeEmployeeId) || employee.Id.ToString() != r.ExcludeEmployeeId)
                {
                    departmentRequests.Add((req, employee));
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
                        EmployeeNames = [.. employeesOnLeave.Select(x => $"{x.Employee.FirstName} {x.Employee.LastName}")]
                    };
                }

                currentDate = currentDate.AddDays(1);
            }

            return new BaseResponse<IEnumerable<TeamLeaveConflictDto>>
            {
                Data = conflicts.Values.Where(c => c.HasConflict).OrderBy(c => c.Date)
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

        return new BaseResponse<IEnumerable<TeamLeaveConflictDto>>
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