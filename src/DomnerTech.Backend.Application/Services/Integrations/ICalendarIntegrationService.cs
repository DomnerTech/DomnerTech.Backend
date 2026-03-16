using MongoDB.Bson;

namespace DomnerTech.Backend.Application.Services.Integrations;

/// <summary>
/// Interface for calendar integration (Google Calendar, Outlook).
/// </summary>
public interface ICalendarIntegrationService : IBaseService
{
    /// <summary>
    /// Creates a calendar event for approved leave.
    /// </summary>
    Task CreateLeaveEventAsync(ObjectId employeeId, string title, DateTime startDate, DateTime endDate, string description, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates a calendar event.
    /// </summary>
    Task UpdateLeaveEventAsync(string eventId, string title, DateTime startDate, DateTime endDate, string description, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes a calendar event when leave is cancelled.
    /// </summary>
    Task DeleteLeaveEventAsync(string eventId, CancellationToken cancellationToken = default);
}