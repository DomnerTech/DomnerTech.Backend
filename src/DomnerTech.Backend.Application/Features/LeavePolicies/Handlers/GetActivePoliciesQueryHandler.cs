using Bas24.CommandQuery;
using DomnerTech.Backend.Application.DTOs;
using DomnerTech.Backend.Application.DTOs.Leaves.LeavePolicies;
using DomnerTech.Backend.Application.Errors;
using DomnerTech.Backend.Application.IRepo;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace DomnerTech.Backend.Application.Features.LeavePolicies.Handlers;

public sealed class GetActivePoliciesQueryHandler(
    ILogger<GetActivePoliciesQueryHandler> logger,
    ILeavePolicyRepo leavePolicyRepo) : IRequestHandler<GetActivePoliciesQuery, BaseResponse<List<LeavePolicyDto>>>
{
    public async Task<BaseResponse<List<LeavePolicyDto>>> Handle(GetActivePoliciesQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var entities = await leavePolicyRepo.GetAllActiveAsync(cancellationToken);
            var dtos = entities.Select(e => new LeavePolicyDto
            {
                Id = e.Id.ToString(),
                PolicyName = e.PolicyName,
                LeaveTypeId = e.LeaveTypeId?.ToString(),
                MinimumNoticeDays = e.MinimumNoticeDays,
                MaxConsecutiveDays = e.MaxConsecutiveDays,
                IncludeWeekends = e.IncludeWeekends,
                IncludePublicHolidays = e.IncludePublicHolidays,
                AllowDuringProbation = e.AllowDuringProbation,
                ProbationPeriodMonths = e.ProbationPeriodMonths,
                AllowNegativeBalance = e.AllowNegativeBalance,
                MaxNegativeBalance = e.MaxNegativeBalance,
                AllowBackdatedRequests = e.AllowBackdatedRequests,
                MaxBackdatedDays = e.MaxBackdatedDays,
                IsActive = e.IsActive,
                EffectiveFrom = e.EffectiveFrom,
                EffectiveTo = e.EffectiveTo,
                CreatedAt = e.CreatedAt,
                UpdatedAt = e.UpdatedAt
            }).ToList();

            return new BaseResponse<List<LeavePolicyDto>> { Data = dtos };
        }
        catch (OperationCanceledException)
        {
            throw;
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error getting active policies: {Error}", e.Message);
        }

        return new BaseResponse<List<LeavePolicyDto>>
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