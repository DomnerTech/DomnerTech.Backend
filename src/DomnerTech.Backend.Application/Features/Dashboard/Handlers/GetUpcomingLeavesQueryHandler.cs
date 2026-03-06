using Bas24.CommandQuery;
using DomnerTech.Backend.Application.DTOs;
using DomnerTech.Backend.Application.DTOs.Leaves.Dashboard;
using DomnerTech.Backend.Application.Errors;
using DomnerTech.Backend.Application.IRepo;
using DomnerTech.Backend.Domain.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace DomnerTech.Backend.Application.Features.Dashboard.Handlers;

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