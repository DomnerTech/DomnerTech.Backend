using DomnerTech.Backend.Application.Services.Integrations;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;

namespace DomnerTech.Backend.Infrastructure.Services.Integrations;

/// <summary>
/// Stub implementation for calendar integration.
/// TODO: Implement actual integration with Google Calendar/Outlook.
/// </summary>
public sealed class CalendarIntegrationService(
    ILogger<CalendarIntegrationService> logger) : ICalendarIntegrationService
{
    public async Task CreateLeaveEventAsync(ObjectId employeeId, string title, DateTime startDate, DateTime endDate, string description, CancellationToken cancellationToken = default)
    {
        // TODO: Implement Google Calendar / Outlook API integration
        logger.LogInformation(
            "Creating calendar event '{Title}' for employee {EmployeeId} from {StartDate} to {EndDate}",
            title, employeeId, startDate, endDate);
        
        await Task.CompletedTask;
    }

    public async Task UpdateLeaveEventAsync(string eventId, string title, DateTime startDate, DateTime endDate, string description, CancellationToken cancellationToken = default)
    {
        // TODO: Implement Google Calendar / Outlook API integration
        logger.LogInformation(
            "Updating calendar event {EventId}: '{Title}' from {StartDate} to {EndDate}",
            eventId, title, startDate, endDate);
        
        await Task.CompletedTask;
    }

    public async Task DeleteLeaveEventAsync(string eventId, CancellationToken cancellationToken = default)
    {
        // TODO: Implement Google Calendar / Outlook API integration
        logger.LogInformation("Deleting calendar event {EventId}", eventId);
        
        await Task.CompletedTask;
    }
}