using Bas24.CommandQuery;
using DomnerTech.Backend.Application.DTOs;
using DomnerTech.Backend.Application.DTOs.Leaves.Dashboard;
using DomnerTech.Backend.Application.Errors;
using DomnerTech.Backend.Application.IRepo;
using DomnerTech.Backend.Domain.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace DomnerTech.Backend.Application.Features.Dashboard.Handlers;

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

                if (employee == null) continue;
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
