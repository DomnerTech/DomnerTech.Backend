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
    IHolidayRepo holidayRepo) : IRequestHandler<GetHolidaysByYearQuery, BaseResponse<IEnumerable<HolidayDto>>>
{
    public async Task<BaseResponse<IEnumerable<HolidayDto>>> Handle(GetHolidaysByYearQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var entities = await holidayRepo.GetByYearAsync(request.Year, cancellationToken);
            return new BaseResponse<IEnumerable<HolidayDto>>
            {
                Data = entities
                    .Select(e => e.ToDto())
                    .OrderBy(x => x.Date)
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
