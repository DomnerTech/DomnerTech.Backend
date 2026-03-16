using DomnerTech.Backend.Application.Services.Integrations;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;

namespace DomnerTech.Backend.Infrastructure.Services.Integrations;

/// <summary>
/// Stub implementation for attendance integration.
/// TODO: Implement actual integration with attendance system.
/// </summary>
public sealed class AttendanceIntegrationService(
    ILogger<AttendanceIntegrationService> logger) : IAttendanceIntegrationService
{
    public async Task MarkAttendanceAsLeaveAsync(ObjectId employeeId, DateTime startDate, DateTime endDate, string leaveType, CancellationToken cancellationToken = default)
    {
        // TODO: Implement actual attendance system API call
        logger.LogInformation(
            "Marking attendance as {LeaveType} for employee {EmployeeId} from {StartDate} to {EndDate}",
            leaveType, employeeId, startDate, endDate);
        
        await Task.CompletedTask;
    }

    public async Task RemoveLeaveAttendanceAsync(ObjectId employeeId, DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default)
    {
        // TODO: Implement actual attendance system API call
        logger.LogInformation(
            "Removing leave attendance for employee {EmployeeId} from {StartDate} to {EndDate}",
            employeeId, startDate, endDate);
        
        await Task.CompletedTask;
    }
}