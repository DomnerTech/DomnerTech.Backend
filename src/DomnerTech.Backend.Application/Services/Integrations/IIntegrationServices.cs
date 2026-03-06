using MongoDB.Bson;

namespace DomnerTech.Backend.Application.Services.Integrations;

/// <summary>
/// Interface for attendance system integration.
/// </summary>
public interface IAttendanceIntegrationService : IBaseService
{
    /// <summary>
    /// Marks attendance as leave for approved leave request.
    /// </summary>
    Task MarkAttendanceAsLeaveAsync(ObjectId employeeId, DateTime startDate, DateTime endDate, string leaveType, CancellationToken cancellationToken = default);

    /// <summary>
    /// Removes leave attendance marks when leave is cancelled.
    /// </summary>
    Task RemoveLeaveAttendanceAsync(ObjectId employeeId, DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default);
}

/// <summary>
/// Interface for payroll system integration.
/// </summary>
public interface IPayrollIntegrationService : IBaseService
{
    /// <summary>
    /// Notifies payroll system about unpaid leave deduction.
    /// </summary>
    Task DeductUnpaidLeaveAsync(ObjectId employeeId, DateTime startDate, DateTime endDate, decimal days, CancellationToken cancellationToken = default);

    /// <summary>
    /// Reverts unpaid leave deduction when leave is cancelled.
    /// </summary>
    Task RevertLeaveDeductionAsync(ObjectId employeeId, DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default);
}

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
