using Bas24.CommandQuery;
using DomnerTech.Backend.Application.DTOs;
using DomnerTech.Backend.Application.DTOs.Leaves.Dashboard;
using DomnerTech.Backend.Application.Errors;
using DomnerTech.Backend.Application.IRepo;
using DomnerTech.Backend.Domain.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace DomnerTech.Backend.Application.Features.Dashboard.Handlers;

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