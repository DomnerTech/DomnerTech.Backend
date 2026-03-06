using DomnerTech.Backend.Application.Extensions;
using DomnerTech.Backend.Application.IRepo;
using DomnerTech.Backend.Application.Services;
using DomnerTech.Backend.Domain.Entities;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;

namespace DomnerTech.Backend.Infrastructure.Services;

public sealed class NotificationService(
    ILogger<NotificationService> logger,
    INotificationRepo notificationRepo,
    ITenantService tenantService) : INotificationService
{
    public async Task SendNotificationAsync(
        ObjectId recipientId,
        string type,
        string title,
        string message,
        ObjectId? relatedEntityId = null,
        bool sendEmail = false,
        bool sendSms = false,
        string priority = "Normal",
        CancellationToken cancellationToken = default)
    {
        try
        {
            var date = DateTime.UtcNow;
            var notification = new NotificationEntity
            {
                Id = ObjectId.GenerateNewId(),
                CompanyId = tenantService.CompanyId.ToObjectId(),
                RecipientId = recipientId,
                Type = type,
                Title = title,
                Message = message,
                RelatedEntityId = relatedEntityId,
                IsRead = false,
                EmailSent = false,
                SmsSent = false,
                Priority = priority,
                CreatedAt = date,
                UpdatedAt = date
            };

            await notificationRepo.CreateAsync(notification, cancellationToken);

            // TODO: Implement actual email/SMS sending
            if (sendEmail)
            {
                logger.LogInformation("Email notification would be sent to {RecipientId}: {Title}", recipientId, title);
                notification.EmailSent = true;
            }

            if (sendSms)
            {
                logger.LogInformation("SMS notification would be sent to {RecipientId}: {Title}", recipientId, title);
                notification.SmsSent = true;
            }

            if (sendEmail || sendSms)
            {
                await notificationRepo.UpdateAsync(notification, cancellationToken);
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error sending notification to {RecipientId}", recipientId);
        }
    }

    public async Task SendLeaveRequestSubmittedAsync(ObjectId employeeId, ObjectId leaveRequestId, CancellationToken cancellationToken = default)
    {
        await SendNotificationAsync(
            employeeId,
            "LeaveRequestSubmitted",
            "Leave Request Submitted",
            "Your leave request has been submitted successfully and is pending approval.",
            leaveRequestId,
            sendEmail: true,
            priority: "Normal",
            cancellationToken: cancellationToken);
    }

    public async Task SendLeaveRequestApprovedAsync(ObjectId employeeId, ObjectId leaveRequestId, CancellationToken cancellationToken = default)
    {
        await SendNotificationAsync(
            employeeId,
            "LeaveRequestApproved",
            "Leave Request Approved",
            "Your leave request has been approved.",
            leaveRequestId,
            sendEmail: true,
            sendSms: true,
            priority: "High",
            cancellationToken: cancellationToken);
    }

    public async Task SendLeaveRequestRejectedAsync(ObjectId employeeId, ObjectId leaveRequestId, string reason, CancellationToken cancellationToken = default)
    {
        await SendNotificationAsync(
            employeeId,
            "LeaveRequestRejected",
            "Leave Request Rejected",
            $"Your leave request has been rejected. Reason: {reason}",
            leaveRequestId,
            sendEmail: true,
            priority: "High",
            cancellationToken: cancellationToken);
    }

    public async Task SendPendingApprovalReminderAsync(ObjectId approverId, ObjectId leaveRequestId, CancellationToken cancellationToken = default)
    {
        await SendNotificationAsync(
            approverId,
            "PendingApprovalReminder",
            "Pending Leave Approval",
            "You have a pending leave request waiting for your approval.",
            leaveRequestId,
            sendEmail: true,
            priority: "Normal",
            cancellationToken: cancellationToken);
    }

    public async Task SendUpcomingLeaveReminderAsync(ObjectId employeeId, ObjectId leaveRequestId, DateTime startDate, CancellationToken cancellationToken = default)
    {
        var daysUntil = (startDate.Date - DateTime.UtcNow.Date).Days;
        await SendNotificationAsync(
            employeeId,
            "UpcomingLeaveReminder",
            "Upcoming Leave Reminder",
            $"Your approved leave starts in {daysUntil} days on {startDate:MMM dd, yyyy}.",
            leaveRequestId,
            sendEmail: true,
            priority: "Normal",
            cancellationToken: cancellationToken);
    }
}
