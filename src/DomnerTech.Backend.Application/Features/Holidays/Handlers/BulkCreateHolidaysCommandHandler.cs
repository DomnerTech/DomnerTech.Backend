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

namespace DomnerTech.Backend.Application.Features.Holidays.Handlers;

/// <summary>
/// Handler for bulk creating holidays.
/// </summary>
public sealed class BulkCreateHolidaysCommandHandler(
    ILogger<BulkCreateHolidaysCommandHandler> logger,
    IHolidayRepo holidayRepo,
    ITenantService tenantService) : IRequestHandler<BulkCreateHolidaysCommand, BaseResponse<int>>
{
    public async Task<BaseResponse<int>> Handle(BulkCreateHolidaysCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var date = DateTime.UtcNow;
            var companyId = tenantService.CompanyId.ToObjectId();

            var entities = request.Dto.Holidays.Select(h => new HolidayEntity
            {
                Id = ObjectId.GenerateNewId(),
                CompanyId = companyId,
                Name = h.Name,
                Description = h.Description,
                Date = h.Date.Date,
                Type = h.Type,
                IsRecurring = h.IsRecurring,
                CountryCode = h.CountryCode?.ToUpperInvariant(),
                Region = h.Region,
                IsActive = true,
                CreatedAt = date,
                UpdatedAt = date,
                IsDeleted = false
            }).ToList();

            var count = await holidayRepo.BulkCreateAsync(entities, cancellationToken);

            return new BaseResponse<int>
            {
                Data = count
            };
        }
        catch (OperationCanceledException)
        {
            throw;
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error bulk creating holidays: {Error}", e.Message);
        }

        return new BaseResponse<int>
        {
            Data = 0,
            Status = new ResponseStatus
            {
                StatusCode = StatusCodes.Status500InternalServerError,
                ErrorCode = ErrorCodes.SystemError
            }
        };
    }
}
