using MongoDB.Bson;

namespace DomnerTech.Backend.Application.Services;

/// <summary>
/// Service for audit logging.
/// </summary>
public interface IAuditLogService : IBaseService
{
    /// <summary>
    /// Logs an action to the audit trail.
    /// </summary>
    Task LogActionAsync(
        ObjectId userId,
        string userName,
        string action,
        string entityType,
        ObjectId? entityId,
        string description,
        string? oldValues = null,
        string? newValues = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Logs leave request creation.
    /// </summary>
    Task LogLeaveRequestCreatedAsync(ObjectId userId, string userName, ObjectId leaveRequestId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Logs leave request approval.
    /// </summary>
    Task LogLeaveRequestApprovedAsync(ObjectId userId, string userName, ObjectId leaveRequestId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Logs leave request rejection.
    /// </summary>
    Task LogLeaveRequestRejectedAsync(ObjectId userId, string userName, ObjectId leaveRequestId, string reason, CancellationToken cancellationToken = default);

    /// <summary>
    /// Logs leave request cancellation.
    /// </summary>
    Task LogLeaveRequestCancelledAsync(ObjectId userId, string userName, ObjectId leaveRequestId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Logs policy update.
    /// </summary>
    Task LogPolicyUpdatedAsync(ObjectId userId, string userName, ObjectId policyId, string oldValues, string newValues, CancellationToken cancellationToken = default);
}
