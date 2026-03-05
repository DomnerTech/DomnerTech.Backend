using DomnerTech.Backend.Domain.Entities;

namespace DomnerTech.Backend.Application.Services;

/// <summary>
/// Service for holiday-related business logic.
/// </summary>
public interface IHolidayService : IBaseService
{
    /// <summary>
    /// Calculates the number of working days between two dates, excluding holidays and weekends.
    /// </summary>
    Task<int> CalculateWorkingDaysAsync(DateTime startDate, DateTime endDate, bool includeWeekends = false, CancellationToken cancellationToken = default);

    /// <summary>
    /// Checks if a specific date is a holiday.
    /// </summary>
    Task<bool> IsHolidayAsync(DateTime date, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets holidays in a date range.
    /// </summary>
    Task<List<HolidayEntity>> GetHolidaysInRangeAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default);
}
