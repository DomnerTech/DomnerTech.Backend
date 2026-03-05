using DomnerTech.Backend.Application.IRepo;
using DomnerTech.Backend.Application.Services;
using DomnerTech.Backend.Domain.Entities;

namespace DomnerTech.Backend.Infrastructure.Services;

/// <summary>
/// Implementation of holiday service.
/// </summary>
public sealed class HolidayService(IHolidayRepo holidayRepo) : IHolidayService
{
    public async Task<int> CalculateWorkingDaysAsync(DateTime startDate, DateTime endDate, bool includeWeekends = false, CancellationToken cancellationToken = default)
    {
        var holidays = await holidayRepo.GetInRangeAsync(startDate.Date, endDate.Date, cancellationToken);
        var holidayDates = holidays.Select(h => h.Date.Date).ToHashSet();

        int workingDays = 0;
        var currentDate = startDate.Date;

        while (currentDate <= endDate.Date)
        {
            bool isWeekend = currentDate.DayOfWeek == DayOfWeek.Saturday || currentDate.DayOfWeek == DayOfWeek.Sunday;
            bool isHoliday = holidayDates.Contains(currentDate);

            if (!isHoliday)
            {
                if (includeWeekends || !isWeekend)
                {
                    workingDays++;
                }
            }

            currentDate = currentDate.AddDays(1);
        }

        return workingDays;
    }

    public async Task<bool> IsHolidayAsync(DateTime date, CancellationToken cancellationToken = default)
    {
        return await holidayRepo.ExistsOnDateAsync(date.Date, null, cancellationToken);
    }

    public async Task<List<HolidayEntity>> GetHolidaysInRangeAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default)
    {
        return await holidayRepo.GetInRangeAsync(startDate.Date, endDate.Date, cancellationToken);
    }
}
