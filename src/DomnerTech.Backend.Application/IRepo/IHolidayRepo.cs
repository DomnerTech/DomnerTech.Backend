using DomnerTech.Backend.Domain.Entities;
using MongoDB.Bson;

namespace DomnerTech.Backend.Application.IRepo;

/// <summary>
/// Repository interface for holiday operations.
/// </summary>
public interface IHolidayRepo : IBaseRepo
{
    /// <summary>
    /// Creates a new holiday.
    /// </summary>
    Task<ObjectId> CreateAsync(HolidayEntity entity, CancellationToken cancellationToken = default);

    /// <summary>
    /// Creates multiple holidays.
    /// </summary>
    Task<int> BulkCreateAsync(List<HolidayEntity> entities, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing holiday.
    /// </summary>
    Task UpdateAsync(HolidayEntity entity, CancellationToken cancellationToken = default);

    /// <summary>
    /// Soft deletes a holiday.
    /// </summary>
    Task DeleteAsync(ObjectId id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a holiday by ID.
    /// </summary>
    Task<HolidayEntity?> GetByIdAsync(ObjectId id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets holidays for a specific year.
    /// </summary>
    Task<List<HolidayEntity>> GetByYearAsync(int year, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets upcoming holidays from a specific date.
    /// </summary>
    Task<List<HolidayEntity>> GetUpcomingAsync(DateTime fromDate, int count, CancellationToken cancellationToken = default);

    /// <summary>
    /// Checks if a holiday exists on a specific date.
    /// </summary>
    Task<bool> ExistsOnDateAsync(DateTime date, ObjectId? excludeId = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets holidays in a date range.
    /// </summary>
    Task<List<HolidayEntity>> GetInRangeAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default);
}
