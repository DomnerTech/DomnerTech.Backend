using Bas24.CommandQuery;
using DomnerTech.Backend.Application.DTOs;
using DomnerTech.Backend.Application.DTOs.Leaves.TeamLeave;
using DomnerTech.Backend.Application.Errors;
using DomnerTech.Backend.Application.IRepo;
using DomnerTech.Backend.Domain.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace DomnerTech.Backend.Application.Features.TeamLeave.Handlers;

public sealed class GetTeamLeaveCalendarQueryHandler(
    ILogger<GetTeamLeaveCalendarQueryHandler> logger,
    ILeaveRequestRepo leaveRequestRepo,
    IEmployeeRepo employeeRepo,
    ILeaveTypeRepo leaveTypeRepo) : IRequestHandler<GetTeamLeaveCalendarQuery, BaseResponse<IEnumerable<TeamLeaveCalendarDto>>>
{
    public async Task<BaseResponse<IEnumerable<TeamLeaveCalendarDto>>> Handle(GetTeamLeaveCalendarQuery request, CancellationToken cancellationToken)
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

            return new BaseResponse<IEnumerable<TeamLeaveCalendarDto>>
            {
                Data = result.OrderBy(x => x.StartDate)
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

        return new BaseResponse<IEnumerable<TeamLeaveCalendarDto>>
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