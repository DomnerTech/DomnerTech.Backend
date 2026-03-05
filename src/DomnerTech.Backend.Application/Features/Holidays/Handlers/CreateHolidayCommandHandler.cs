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

namespace DomnerTech.Backend.Application.Features.Holidays.Handlers;

/// <summary>
/// Handler for creating a new holiday.
/// </summary>
public sealed class CreateHolidayCommandHandler(
    ILogger<CreateHolidayCommandHandler> logger,
    IHolidayRepo holidayRepo,
    ITenantService tenantService) : IRequestHandler<CreateHolidayCommand, BaseResponse<string>>
{
    public async Task<BaseResponse<string>> Handle(CreateHolidayCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var r = request.Dto;

            if (await holidayRepo.ExistsOnDateAsync(r.Date.Date, null, cancellationToken))
            {
                throw new ConflictException("A holiday already exists on this date");
            }

            var date = DateTime.UtcNow;
            var entity = new HolidayEntity
            {
                Id = ObjectId.GenerateNewId(),
                CompanyId = tenantService.CompanyId.ToObjectId(),
                Name = r.Name,
                Description = r.Description,
                Date = r.Date.Date,
                Type = r.Type,
                IsRecurring = r.IsRecurring,
                CountryCode = r.CountryCode?.ToUpperInvariant(),
                Region = r.Region,
                IsActive = true,
                CreatedAt = date,
                UpdatedAt = date,
                IsDeleted = false
            };

            var id = await holidayRepo.CreateAsync(entity, cancellationToken);

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
            logger.LogError(e, "Error creating holiday: {Error}", e.Message);
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
