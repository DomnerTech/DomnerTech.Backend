using Bas24.CommandQuery;
using DomnerTech.Backend.Application.DTOs;
using DomnerTech.Backend.Application.DTOs.Leaves.Reports;
using DomnerTech.Backend.Application.Errors;
using DomnerTech.Backend.Application.IRepo;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace DomnerTech.Backend.Application.Features.Reports.Handlers;

/// <summary>
/// Handler for GetLeaveUsageReportQuery.
/// </summary>
public sealed class GetLeaveUsageReportQueryHandler(
    ILogger<GetLeaveUsageReportQueryHandler> logger,
    ILeaveBalanceRepo leaveBalanceRepo,
    ILeaveTypeRepo leaveTypeRepo,
    IEmployeeRepo employeeRepo) : IRequestHandler<GetLeaveUsageReportQuery, BaseResponse<List<LeaveUsageReportDto>>>
{
    public async Task<BaseResponse<List<LeaveUsageReportDto>>> Handle(GetLeaveUsageReportQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var balances = await leaveBalanceRepo.GetAllByYearAsync(request.Year, cancellationToken);
            var leaveTypes = await leaveTypeRepo.GetAllActiveAsync(cancellationToken);
            var report = new List<LeaveUsageReportDto>();

            foreach (var balance in balances)
            {
                var employee = await employeeRepo.GetByIdAsync(balance.EmployeeId, cancellationToken);
                if (employee == null) continue;

                // Filter by department if specified
                if (!string.IsNullOrWhiteSpace(request.Department) && 
                    !employee.Department.Equals(request.Department, StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                var leaveType = leaveTypes.FirstOrDefault(lt => lt.Id == balance.LeaveTypeId);
                if (leaveType == null) continue;

                var usagePercentage = balance.Allowance.TotalAllowance > 0
                    ? (balance.Allowance.UsedDays / balance.Allowance.TotalAllowance) * 100
                    : 0;

                report.Add(new LeaveUsageReportDto
                {
                    EmployeeId = balance.EmployeeId.ToString(),
                    EmployeeName = $"{employee.FirstName} {employee.LastName}",
                    Department = employee.Department,
                    LeaveTypeName = leaveType.Name,
                    TotalAllowance = balance.Allowance.TotalAllowance,
                    UsedDays = balance.Allowance.UsedDays,
                    RemainingDays = balance.Allowance.RemainingDays,
                    UsagePercentage = Math.Round(usagePercentage, 2)
                });
            }

            return new BaseResponse<List<LeaveUsageReportDto>> { Data = report };
        }
        catch (OperationCanceledException)
        {
            throw;
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error generating leave usage report: {Error}", e.Message);
        }

        return new BaseResponse<List<LeaveUsageReportDto>>
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
