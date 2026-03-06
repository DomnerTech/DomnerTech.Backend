using MongoDB.Bson;

namespace DomnerTech.Backend.Application.Services;

/// <summary>
/// Service for sending notifications via multiple channels.
/// </summary>
public interface INotificationService : IBaseService
{
    /// <summary>
    /// Sends a notification to a recipient.
    /// </summary>
    Task SendNotificationAsync(
        ObjectId recipientId,
        string type,
        string title,
        string message,
        ObjectId? relatedEntityId = null,
        bool sendEmail = false,
        bool sendSms = false,
        string priority = "Normal",
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Sends a leave request submitted notification.
    /// </summary>
    Task SendLeaveRequestSubmittedAsync(ObjectId employeeId, ObjectId leaveRequestId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Sends a leave request approved notification.
    /// </summary>
    Task SendLeaveRequestApprovedAsync(ObjectId employeeId, ObjectId leaveRequestId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Sends a leave request rejected notification.
    /// </summary>
    Task SendLeaveRequestRejectedAsync(ObjectId employeeId, ObjectId leaveRequestId, string reason, CancellationToken cancellationToken = default);

    /// <summary>
    /// Sends a pending approval reminder to approver.
    /// </summary>
    Task SendPendingApprovalReminderAsync(ObjectId approverId, ObjectId leaveRequestId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Sends an upcoming leave reminder.
    /// </summary>
    Task SendUpcomingLeaveReminderAsync(ObjectId employeeId, ObjectId leaveRequestId, DateTime startDate, CancellationToken cancellationToken = default);
}
