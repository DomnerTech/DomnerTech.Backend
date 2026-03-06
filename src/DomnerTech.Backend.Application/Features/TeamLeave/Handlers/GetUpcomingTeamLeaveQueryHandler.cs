using Bas24.CommandQuery;
using DomnerTech.Backend.Application.DTOs;
using DomnerTech.Backend.Application.DTOs.Leaves.TeamLeave;
using DomnerTech.Backend.Application.Errors;
using DomnerTech.Backend.Application.IRepo;
using DomnerTech.Backend.Domain.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace DomnerTech.Backend.Application.Features.TeamLeave.Handlers;

public sealed class GetUpcomingTeamLeaveQueryHandler(
    ILogger<GetUpcomingTeamLeaveQueryHandler> logger,
    ILeaveRequestRepo leaveRequestRepo,
    IEmployeeRepo employeeRepo,
    ILeaveTypeRepo leaveTypeRepo) : IRequestHandler<GetUpcomingTeamLeaveQuery, BaseResponse<IEnumerable<TeamLeaveCalendarDto>>>
{
    public async Task<BaseResponse<IEnumerable<TeamLeaveCalendarDto>>> Handle(GetUpcomingTeamLeaveQuery request, CancellationToken cancellationToken)
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
            logger.LogError(e, "Error getting upcoming team leave: {Error}", e.Message);
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
