using Bas24.CommandQuery;
using DomnerTech.Backend.Application.DTOs;
using DomnerTech.Backend.Application.DTOs.Leaves.Holidays;
using DomnerTech.Backend.Application.Errors;
using DomnerTech.Backend.Application.IRepo;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace DomnerTech.Backend.Application.Features.Holidays.Handlers;

/// <summary>
/// Handler for getting holidays by year.
/// </summary>
public sealed class GetHolidaysByYearQueryHandler(
    ILogger<GetHolidaysByYearQueryHandler> logger,
    IHolidayRepo holidayRepo) : IRequestHandler<GetHolidaysByYearQuery, BaseResponse<List<HolidayDto>>>
{
    public async Task<BaseResponse<List<HolidayDto>>> Handle(GetHolidaysByYearQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var entities = await holidayRepo.GetByYearAsync(request.Year, cancellationToken);

            var dtos = entities.Select(e => new HolidayDto
            {
                Id = e.Id.ToString(),
                Name = e.Name,
                Description = e.Description,
                Date = e.Date,
                Type = e.Type,
                IsRecurring = e.IsRecurring,
                CountryCode = e.CountryCode,
                Region = e.Region,
                IsActive = e.IsActive,
                CreatedAt = e.CreatedAt,
                UpdatedAt = e.UpdatedAt
            })
            .OrderBy(x => x.Date)
            .ToList();

            return new BaseResponse<List<HolidayDto>>
            {
                Data = dtos
            };
        }
        catch (OperationCanceledException)
        {
            throw;
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error getting holidays by year: {Error}", e.Message);
        }

        return new BaseResponse<List<HolidayDto>>
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
