using Bas24.CommandQuery;
using DomnerTech.Backend.Application.DTOs;
using DomnerTech.Backend.Application.Errors;
using DomnerTech.Backend.Application.Exceptions;
using DomnerTech.Backend.Application.IRepo;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;

namespace DomnerTech.Backend.Application.Features.LeavePolicies.Handlers;

public sealed class UpdateLeavePolicyCommandHandler(
    ILogger<UpdateLeavePolicyCommandHandler> logger,
    ILeavePolicyRepo leavePolicyRepo) : IRequestHandler<UpdateLeavePolicyCommand, BaseResponse<bool>>
{
    public async Task<BaseResponse<bool>> Handle(UpdateLeavePolicyCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var r = request.Dto;
            var policyId = ObjectId.Parse(r.Id);

            var existing = await leavePolicyRepo.GetByIdAsync(policyId, cancellationToken);
            if (existing is null)
            {
                throw new NotFoundException("Leave policy not found");
            }

            existing.PolicyName = r.Name;
            existing.MinimumNoticeDays = r.MinimumNoticeDays;
            existing.MaxConsecutiveDays = r.MaxConsecutiveDays;
            existing.IncludeWeekends = r.IncludeWeekends;
            existing.IncludePublicHolidays = r.IncludePublicHolidays;
            existing.AllowDuringProbation = r.AllowDuringProbation;
            existing.ProbationPeriodMonths = r.ProbationPeriodMonths;
            existing.AllowNegativeBalance = r.AllowNegativeBalance;
            existing.MaxNegativeBalance = r.MaxNegativeBalance;
            existing.AllowBackdatedRequests = r.AllowBackdatedRequests;
            existing.MaxBackdatedDays = r.MaxBackdatedDays;
            existing.IsActive = r.IsActive;
            existing.EffectiveTo = r.EffectiveTo;
            existing.UpdatedAt = DateTime.UtcNow;

            await leavePolicyRepo.UpdateAsync(existing, cancellationToken);

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
            logger.LogError(e, "Error updating leave policy: {Error}", e.Message);
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