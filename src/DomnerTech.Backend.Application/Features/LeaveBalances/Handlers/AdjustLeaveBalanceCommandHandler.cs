using Bas24.CommandQuery;
using DomnerTech.Backend.Application.DTOs;
using DomnerTech.Backend.Application.Errors;
using DomnerTech.Backend.Application.Exceptions;
using DomnerTech.Backend.Application.IRepo;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;

namespace DomnerTech.Backend.Application.Features.LeaveBalances.Handlers;

public sealed class AdjustLeaveBalanceCommandHandler(
    ILogger<AdjustLeaveBalanceCommandHandler> logger,
    ILeaveBalanceRepo leaveBalanceRepo) : IRequestHandler<AdjustLeaveBalanceCommand, BaseResponse<bool>>
{
    public async Task<BaseResponse<bool>> Handle(AdjustLeaveBalanceCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var r = request.Dto;
            var employeeId = ObjectId.Parse(r.EmployeeId);
            var leaveTypeId = ObjectId.Parse(r.LeaveTypeId);

            var balance = await leaveBalanceRepo.GetByEmployeeAndTypeAsync(employeeId, leaveTypeId, r.Year, cancellationToken);
            if (balance == null)
            {
                throw new NotFoundException("Leave balance not found");
            }

            balance.Allowance.TotalAllowance += r.AdjustmentDays;
            balance.UpdatedAt = DateTime.UtcNow;

            await leaveBalanceRepo.UpdateAsync(balance, cancellationToken);

            return new BaseResponse<bool> { Data = true };
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
            logger.LogError(e, "Error adjusting leave balance: {Error}", e.Message);
        }

        return new BaseResponse<bool>
        {
            Data = false,
            Status = new ResponseStatus
            {
                StatusCode = StatusCodes.Status500InternalServerError,
                ErrorCode = ErrorCodes.SystemError
            }
        };
    }
}