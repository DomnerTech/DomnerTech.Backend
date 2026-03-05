using Bas24.CommandQuery;
using DomnerTech.Backend.Application.DTOs;
using DomnerTech.Backend.Application.Errors;
using DomnerTech.Backend.Application.Exceptions;
using DomnerTech.Backend.Application.Extensions;
using DomnerTech.Backend.Application.IRepo;
using DomnerTech.Backend.Application.Services;
using DomnerTech.Backend.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;

namespace DomnerTech.Backend.Application.Features.LeaveTypes.Handlers;

/// <summary>
/// Handler for creating a new leave type.
/// </summary>
public sealed class CreateLeaveTypeCommandHandler(
    ILogger<CreateLeaveTypeCommandHandler> logger,
    ILeaveTypeRepo leaveTypeRepo,
    ITenantService tenantService) : IRequestHandler<CreateLeaveTypeCommand, BaseResponse<string>>
{
    public async Task<BaseResponse<string>> Handle(CreateLeaveTypeCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var r = request.Dto;

            // Check if code already exists
            if (await leaveTypeRepo.CodeExistsAsync(r.Code, null, cancellationToken))
            {
                throw new ConflictException("Leave type code already exists");
            }

            var date = DateTime.UtcNow;
            var entity = new LeaveTypeEntity
            {
                Id = ObjectId.GenerateNewId(),
                CompanyId = tenantService.CompanyId.ToObjectId(),
                Name = r.Name,
                Description = r.Description,
                Code = r.Code.ToUpperInvariant(),
                YearlyAllowance = r.YearlyAllowance,
                IsAccrualBased = r.IsAccrualBased,
                MonthlyAccrualDays = r.MonthlyAccrualDays,
                MaxCarryForwardDays = r.MaxCarryForwardDays,
                CarryForwardExpires = r.CarryForwardExpires,
                CarryForwardExpiryDate = r.CarryForwardExpiryDate,
                RequiresDocument = r.RequiresDocument,
                IsPaid = r.IsPaid,
                IsActive = true,
                DisplayOrder = r.DisplayOrder,
                CreatedAt = date,
                UpdatedAt = date,
                IsDeleted = false
            };

            var id = await leaveTypeRepo.CreateAsync(entity, cancellationToken);

            return new BaseResponse<string>
            {
                Data = id.ToString()
            };
        }
        catch (OperationCanceledException)
        {
            throw;
        }
        catch (ConflictException)
        {
            throw;
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error creating leave type: {Error}", e.Message);
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
