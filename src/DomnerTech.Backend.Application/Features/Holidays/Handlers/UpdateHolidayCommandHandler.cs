using Bas24.CommandQuery;
using DomnerTech.Backend.Application.DTOs;
using DomnerTech.Backend.Application.Errors;
using DomnerTech.Backend.Application.Exceptions;
using DomnerTech.Backend.Application.IRepo;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;

namespace DomnerTech.Backend.Application.Features.Holidays.Handlers;

/// <summary>
/// Handler for updating a holiday.
/// </summary>
public sealed class UpdateHolidayCommandHandler(
    ILogger<UpdateHolidayCommandHandler> logger,
    IHolidayRepo holidayRepo) : IRequestHandler<UpdateHolidayCommand, BaseResponse<bool>>
{
    public async Task<BaseResponse<bool>> Handle(UpdateHolidayCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var r = request.Dto;
            var holidayId = ObjectId.Parse(r.Id);

            var existing = await holidayRepo.GetByIdAsync(holidayId, cancellationToken);
            if (existing is null)
            {
                throw new NotFoundException("Holiday not found");
            }

            if (existing.Date.Date != r.Date.Date &&
                await holidayRepo.ExistsOnDateAsync(r.Date.Date, holidayId, cancellationToken))
            {
                throw new ConflictException("A holiday already exists on this date");
            }

            existing.Name = r.Name;
            existing.Description = r.Description;
            existing.Date = r.Date.Date;
            existing.Type = r.Type;
            existing.IsRecurring = r.IsRecurring;
            existing.CountryCode = r.CountryCode?.ToUpperInvariant();
            existing.Region = r.Region;
            existing.IsActive = r.IsActive;
            existing.UpdatedAt = DateTime.UtcNow;

            await holidayRepo.UpdateAsync(existing, cancellationToken);

            return new BaseResponse<bool>
            {
                Data = true
            };
        }
        catch (OperationCanceledException)
        {
            throw;
        }
        catch (NotFoundException)
        {
            throw;
        }
        catch (ConflictException)
        {
            throw;
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error updating holiday: {Error}", e.Message);
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
