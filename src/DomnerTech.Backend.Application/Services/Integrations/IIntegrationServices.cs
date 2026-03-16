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