using Bas24.CommandQuery;
using DomnerTech.Backend.Application.DTOs;
using DomnerTech.Backend.Application.DTOs.Leaves.LeaveBalances;
using DomnerTech.Backend.Application.Errors;
using DomnerTech.Backend.Application.IRepo;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;

namespace DomnerTech.Backend.Application.Features.LeaveBalances.Handlers;

public sealed class GetEmployeeLeaveBalancesQueryHandler(
    ILogger<GetEmployeeLeaveBalancesQueryHandler> logger,
    ILeaveBalanceRepo leaveBalanceRepo,
    ILeaveTypeRepo leaveTypeRepo) : IRequestHandler<GetEmployeeLeaveBalancesQuery, BaseResponse<List<LeaveBalanceSummaryDto>>>
{
    public async Task<BaseResponse<List<LeaveBalanceSummaryDto>>> Handle(GetEmployeeLeaveBalancesQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var employeeId = ObjectId.Parse(request.EmployeeId);
            var balances = await leaveBalanceRepo.GetByEmployeeAsync(employeeId, request.Year, cancellationToken);
            var leaveTypes = await leaveTypeRepo.GetAllActiveAsync(cancellationToken);

            var summary = balances.Select(b =>
            {
                var leaveType = leaveTypes.FirstOrDefault(lt => lt.Id == b.LeaveTypeId);
                return new LeaveBalanceSummaryDto
                {
                    LeaveTypeName = leaveType?.Name ?? "Unknown",
                    LeaveTypeCode = leaveType?.Code ?? "UNKNOWN",
                    TotalAllowance = b.Allowance.TotalAllowance,
                    UsedDays = b.Allowance.UsedDays,
                    RemainingDays = b.Allowance.RemainingDays,
                    CarriedForwardDays = b.Allowance.CarriedForwardDays
                };
            }).ToList();

            return new BaseResponse<List<LeaveBalanceSummaryDto>> { Data = summary };
        }
        catch (OperationCanceledException)
        {
            throw;
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error getting employee leave balances: {Error}", e.Message);
        }

        return new BaseResponse<List<LeaveBalanceSummaryDto>>
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
