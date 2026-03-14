using Bas24.CommandQuery;
using DomnerTech.Backend.Application.DTOs;
using DomnerTech.Backend.Application.Errors;
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
                PolicyName = r.Name,
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