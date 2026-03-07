using Bas24.CommandQuery;
using DomnerTech.Backend.Application.DTOs;
using DomnerTech.Backend.Application.DTOs.Leaves.Reports;
using DomnerTech.Backend.Application.Errors;
using DomnerTech.Backend.Application.Exceptions;
using DomnerTech.Backend.Application.IRepo;
using DomnerTech.Backend.Domain.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;

namespace DomnerTech.Backend.Application.Features.Reports.Handlers;

/// <summary>
/// Handler for GetEmployeeLeaveSummaryQuery.
/// </summary>
public sealed class GetEmployeeLeaveSummaryQueryHandler(
    ILogger<GetEmployeeLeaveSummaryQueryHandler> logger,
    IEmployeeRepo employeeRepo,
    ILeaveBalanceRepo leaveBalanceRepo,
    ILeaveTypeRepo leaveTypeRepo,
    ILeaveRequestRepo leaveRequestRepo) : IRequestHandler<GetEmployeeLeaveSummaryQuery, BaseResponse<EmployeeLeaveSummaryDto>>
{
    public async Task<BaseResponse<EmployeeLeaveSummaryDto>> Handle(GetEmployeeLeaveSummaryQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var employeeId = ObjectId.Parse(request.EmployeeId);
            
            // Get employee
            var employee = await employeeRepo.GetByIdAsync(employeeId, cancellationToken);
            if (employee == null)
            {
                throw new NotFoundException("Employee not found");
            }

            // Get leave balances for the year
            var balances = await leaveBalanceRepo.GetByEmployeeAsync(employeeId, request.Year, cancellationToken);
            var leaveTypes = await leaveTypeRepo.GetAllActiveAsync(cancellationToken);

            // Get all leave requests for the year
            var allRequests = await leaveRequestRepo.GetByEmployeeAsync(employeeId, cancellationToken);
            var requestsThisYear = allRequests
                .Where(r => r.Period.StartDate.Year == request.Year && r.Status == LeaveRequestStatus.Approved)
                .ToList();

            var totalDaysTaken = requestsThisYear.Sum(r => r.Period.TotalDays);

            // Build leave type summaries
            var leaveTypeSummaries = balances.Select(balance =>
            {
                var leaveType = leaveTypes.FirstOrDefault(lt => lt.Id == balance.LeaveTypeId);
                return new LeaveTypeSummary
                {
                    LeaveTypeName = leaveType?.Name ?? "Unknown",
                    Allowance = balance.Allowance.TotalAllowance,
                    Used = balance.Allowance.UsedDays,
                    Remaining = balance.Allowance.RemainingDays
                };
            }).ToList();

            var summary = new EmployeeLeaveSummaryDto
            {
                EmployeeId = employee.Id.ToString(),
                EmployeeName = $"{employee.FirstName} {employee.LastName}",
                LeaveTypes = leaveTypeSummaries,
                TotalRequestsThisYear = requestsThisYear.Count,
                TotalDaysTaken = totalDaysTaken
            };

            return new BaseResponse<EmployeeLeaveSummaryDto> { Data = summary };
        }
        catch (OperationCanceledException)
        {
            throw;
        }
        catch (NotFoundException)
        {
            throw;
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error getting employee leave summary: {Error}", e.Message);
        }

        return new BaseResponse<EmployeeLeaveSummaryDto>
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
