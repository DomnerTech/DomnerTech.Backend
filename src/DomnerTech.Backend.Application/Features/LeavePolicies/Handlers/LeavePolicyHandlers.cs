using Bas24.CommandQuery;
using DomnerTech.Backend.Application.DTOs;
using DomnerTech.Backend.Application.DTOs.Leaves.LeavePolicies;
using DomnerTech.Backend.Application.Errors;
using DomnerTech.Backend.Application.Exceptions;
using DomnerTech.Backend.Application.Extensions;
using DomnerTech.Backend.Application.IRepo;
using DomnerTech.Backend.Application.Services;
using DomnerTech.Backend.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;

namespace DomnerTech.Backend.Application.Features.LeavePolicies.Handlers;

public sealed class CreateLeavePolicyCommandHandler(
    ILogger<CreateLeavePolicyCommandHandler> logger,
    ILeavePolicyRepo leavePolicyRepo,
    ITenantService tenantService) : IRequestHandler<CreateLeavePolicyCommand, BaseResponse<string>>
{
    public async Task<BaseResponse<string>> Handle(CreateLeavePolicyCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var r = request.Dto;
            var date = DateTime.UtcNow;

            var entity = new LeavePolicyEntity
            {
                Id = ObjectId.GenerateNewId(),
                CompanyId = tenantService.CompanyId.ToObjectId(),
                PolicyName = r.PolicyName,
                LeaveTypeId = string.IsNullOrEmpty(r.LeaveTypeId) ? null : ObjectId.Parse(r.LeaveTypeId),
                MinimumNoticeDays = r.MinimumNoticeDays,
                MaxConsecutiveDays = r.MaxConsecutiveDays,
                IncludeWeekends = r.IncludeWeekends,
                IncludePublicHolidays = r.IncludePublicHolidays,
                AllowDuringProbation = r.AllowDuringProbation,
                ProbationPeriodMonths = r.ProbationPeriodMonths,
                AllowNegativeBalance = r.AllowNegativeBalance,
                MaxNegativeBalance = r.MaxNegativeBalance,
                AllowBackdatedRequests = r.AllowBackdatedRequests,
                MaxBackdatedDays = r.MaxBackdatedDays,
                IsActive = true,
                EffectiveFrom = r.EffectiveFrom,
                EffectiveTo = r.EffectiveTo,
                CreatedAt = date,
                UpdatedAt = date,
                IsDeleted = false
            };

            var id = await leavePolicyRepo.CreateAsync(entity, cancellationToken);

            return new BaseResponse<string> { Data = id.ToString() };
        }
        catch (OperationCanceledException)
        {
            throw;
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error creating leave policy: {Error}", e.Message);
        }

        return new BaseResponse<string>
        {
            Data = string.Empty,
            Status = new ResponseStatus
            {
                StatusCode = StatusCodes.Status500InternalServerError,
                ErrorCode = ErrorCodes.SystemError
            }
        };
    }
}

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

            existing.PolicyName = r.PolicyName;
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

public sealed class DeleteLeavePolicyCommandHandler(
    ILogger<DeleteLeavePolicyCommandHandler> logger,
    ILeavePolicyRepo leavePolicyRepo) : IRequestHandler<DeleteLeavePolicyCommand, BaseResponse<bool>>
{
    public async Task<BaseResponse<bool>> Handle(DeleteLeavePolicyCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var policyId = ObjectId.Parse(request.Id);

            var existing = await leavePolicyRepo.GetByIdAsync(policyId, cancellationToken);
            if (existing is null)
            {
                throw new NotFoundException("Leave policy not found");
            }

            await leavePolicyRepo.DeleteAsync(policyId, cancellationToken);

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
            logger.LogError(e, "Error deleting leave policy: {Error}", e.Message);
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

public sealed class GetPolicyByLeaveTypeQueryHandler(
    ILogger<GetPolicyByLeaveTypeQueryHandler> logger,
    ILeavePolicyRepo leavePolicyRepo) : IRequestHandler<GetPolicyByLeaveTypeQuery, BaseResponse<LeavePolicyDto>>
{
    public async Task<BaseResponse<LeavePolicyDto>> Handle(GetPolicyByLeaveTypeQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var leaveTypeId = ObjectId.Parse(request.LeaveTypeId);
            var entity = await leavePolicyRepo.GetByLeaveTypeIdAsync(leaveTypeId, cancellationToken);

            if (entity is null)
            {
                entity = await leavePolicyRepo.GetDefaultPolicyAsync(cancellationToken);
            }

            if (entity is null)
            {
                throw new NotFoundException("No policy found for this leave type");
            }

            var dto = new LeavePolicyDto
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
            logger.LogError(e, "Error getting policy by leave type: {Error}", e.Message);
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
}
