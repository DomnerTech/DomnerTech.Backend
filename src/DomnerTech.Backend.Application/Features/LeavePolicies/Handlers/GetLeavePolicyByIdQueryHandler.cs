using Bas24.CommandQuery;
using DomnerTech.Backend.Application.DTOs;
using DomnerTech.Backend.Application.DTOs.Leaves.LeavePolicies;
using DomnerTech.Backend.Application.Errors;
using DomnerTech.Backend.Application.Exceptions;
using DomnerTech.Backend.Application.IRepo;
using DomnerTech.Backend.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;

namespace DomnerTech.Backend.Application.Features.LeavePolicies.Handlers;

public sealed class GetLeavePolicyByIdQueryHandler(
    ILogger<GetLeavePolicyByIdQueryHandler> logger,
    ILeavePolicyRepo leavePolicyRepo) : IRequestHandler<GetLeavePolicyByIdQuery, BaseResponse<LeavePolicyDto>>
{
    public async Task<BaseResponse<LeavePolicyDto>> Handle(GetLeavePolicyByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var policyId = ObjectId.Parse(request.Id);
            var entity = await leavePolicyRepo.GetByIdAsync(policyId, cancellationToken);

            if (entity is null)
            {
                throw new NotFoundException("Leave policy not found");
            }

            var dto = MapToDto(entity);
            return new BaseResponse<LeavePolicyDto> { Data = dto };
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
            logger.LogError(e, "Error getting leave policy: {Error}", e.Message);
        }

        return new BaseResponse<LeavePolicyDto>
        {
            Data = null!,
            Status = new ResponseStatus
            {
                StatusCode = StatusCodes.Status500InternalServerError,
                ErrorCode = ErrorCodes.SystemError
            }
        };
    }

    private static LeavePolicyDto MapToDto(LeavePolicyEntity entity) => new()
    {
        Id = entity.Id.ToString(),
        PolicyName = entity.PolicyName,
        LeaveTypeId = entity.LeaveTypeId?.ToString(),
        MinimumNoticeDays = entity.MinimumNoticeDays,
        MaxConsecutiveDays = entity.MaxConsecutiveDays,
        IncludeWeekends = entity.IncludeWeekends,
        IncludePublicHolidays = entity.IncludePublicHolidays,
        AllowDuringProbation = entity.AllowDuringProbation,
        ProbationPeriodMonths = entity.ProbationPeriodMonths,
        AllowNegativeBalance = entity.AllowNegativeBalance,
        MaxNegativeBalance = entity.MaxNegativeBalance,
        AllowBackdatedRequests = entity.AllowBackdatedRequests,
        MaxBackdatedDays = entity.MaxBackdatedDays,
        IsActive = entity.IsActive,
        EffectiveFrom = entity.EffectiveFrom,
        EffectiveTo = entity.EffectiveTo,
        CreatedAt = entity.CreatedAt,
        UpdatedAt = entity.UpdatedAt
    };
}