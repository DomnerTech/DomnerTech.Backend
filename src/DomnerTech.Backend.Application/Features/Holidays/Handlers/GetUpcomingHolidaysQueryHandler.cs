using Bas24.CommandQuery;
using DomnerTech.Backend.Application.DTOs;
using DomnerTech.Backend.Application.DTOs.Leaves.Holidays;
using DomnerTech.Backend.Application.Errors;
using DomnerTech.Backend.Application.IRepo;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace DomnerTech.Backend.Application.Features.Holidays.Handlers;

/// <summary>
/// Handler for getting upcoming holidays.
/// </summary>
public sealed class GetUpcomingHolidaysQueryHandler(
    ILogger<GetUpcomingHolidaysQueryHandler> logger,
    IHolidayRepo holidayRepo) : IRequestHandler<GetUpcomingHolidaysQuery, BaseResponse<IEnumerable<HolidayDto>>>
{
    public async Task<BaseResponse<IEnumerable<HolidayDto>>> Handle(GetUpcomingHolidaysQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var fromDate = DateTime.UtcNow.Date;
            var entities = await holidayRepo.GetUpcomingAsync(fromDate, request.Count, cancellationToken);
            return new BaseResponse<IEnumerable<HolidayDto>>
            {
                Data = entities.Select(i => i.ToDto())
            };
        }
        catch (OperationCanceledException)
        {
            throw;
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error getting upcoming holidays: {Error}", e.Message);
        }

        return new BaseResponse<IEnumerable<HolidayDto>>
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
