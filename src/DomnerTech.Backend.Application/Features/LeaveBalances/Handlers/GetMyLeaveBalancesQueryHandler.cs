using Bas24.CommandQuery;
using DomnerTech.Backend.Application.Constants;
using DomnerTech.Backend.Application.DTOs;
using DomnerTech.Backend.Application.DTOs.Leaves.LeaveBalances;
using DomnerTech.Backend.Application.Errors;
using DomnerTech.Backend.Application.Exceptions;
using DomnerTech.Backend.Application.Extensions;
using DomnerTech.Backend.Application.IRepo;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;

namespace DomnerTech.Backend.Application.Features.LeaveBalances.Handlers;

public sealed class GetMyLeaveBalancesQueryHandler(
    ILogger<GetMyLeaveBalancesQueryHandler> logger,
    ILeaveBalanceRepo leaveBalanceRepo,
    ILeaveTypeRepo leaveTypeRepo,
    IHttpContextAccessor httpContextAccessor) : IRequestHandler<GetMyLeaveBalancesQuery, BaseResponse<IEnumerable<LeaveBalanceSummaryDto>>>
{
    public async Task<BaseResponse<IEnumerable<LeaveBalanceSummaryDto>>> Handle(GetMyLeaveBalancesQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var employeeId = httpContextAccessor.HttpContext?.GetClaim(ClaimConstant.EmpId).ToObjectId() ?? ObjectId.Empty;

            if (employeeId == ObjectId.Empty)
            {
                throw new UnauthorizedException();
            }

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
            });

            return new BaseResponse<IEnumerable<LeaveBalanceSummaryDto>>
            {
                Data = summary
            };
        }
        catch (OperationCanceledException)
        {
            throw;
        }
        catch (UnauthorizedException)
        {
            throw;
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error getting my leave balances: {Error}", e.Message);
        }

        return new BaseResponse<IEnumerable<LeaveBalanceSummaryDto>>
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