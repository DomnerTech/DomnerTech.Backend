using DomnerTech.Backend.Application.Constants;
using DomnerTech.Backend.Application.DTOs.Notifications;
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
        SendNotificationParams param,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var date = DateTime.UtcNow;
            var notification = new NotificationEntity
            {
                Id = ObjectId.GenerateNewId(),
                CompanyId = tenantService.CompanyId.ToObjectId(),
                RecipientId = param.RecipientId,
                Type = param.Type,
                Title = param.Title,
                Message = param.Message,
                RelatedEntityId = param.RelatedEntityId,
                IsRead = false,
                EmailSent = false,
                SmsSent = false,
                Priority = param.Priority,
                CreatedAt = date,
                UpdatedAt = date
            };

            await notificationRepo.CreateAsync(notification, cancellationToken);

            // TODO: Implement actual email/SMS sending
            if (param.SendEmail)
            {
                logger.LogInformation("Email notification would be sent to {RecipientId}: {Title}",
                    param.RelatedEntityId, param.Title);
                notification.EmailSent = true;
            }

            if (param.SendSms)
            {
                logger.LogInformation("SMS notification would be sent to {RecipientId}: {Title}",
                    param.RelatedEntityId, param.Title);
                notification.SmsSent = true;
            }

            if (param.SendEmail || param.SendSms)
            {
                await notificationRepo.UpdateAsync(notification, cancellationToken);
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error sending notification to {RecipientId}", param.RecipientId);
        }
    }

    public async Task SendLeaveRequestSubmittedAsync(ObjectId employeeId, ObjectId leaveRequestId, CancellationToken cancellationToken = default)
    {
        await SendNotificationAsync(new SendNotificationParams
        {
            RecipientId = employeeId,
            Type = "LeaveRequestSubmitted",
            Title = "Leave Request Submitted",
            Message = "Your leave request has been submitted successfully and is pending approval.",
            RelatedEntityId = leaveRequestId,
            SendEmail = true,
            Priority = NotificationPriority.Normal
        }, cancellationToken);
    }

    public async Task SendLeaveRequestApprovedAsync(ObjectId employeeId, ObjectId leaveRequestId, CancellationToken cancellationToken = default)
    {
        await SendNotificationAsync(new SendNotificationParams
        {
            RecipientId = employeeId,
            Type = "LeaveRequestApproved",
            Title = "Leave Request Approved",
            Message = "Your leave request has been approved.",
            RelatedEntityId = leaveRequestId,
            SendEmail = true,
            SendSms = true,
            Priority = NotificationPriority.High
        }, cancellationToken);
    }

    public async Task SendLeaveRequestRejectedAsync(ObjectId employeeId, ObjectId leaveRequestId, string reason, CancellationToken cancellationToken = default)
    {
        await SendNotificationAsync(new SendNotificationParams
        {
            RecipientId = employeeId,
            Type = "LeaveRequestRejected",
            Title = "Leave Request Rejected",
            Message = $"Your leave request has been rejected. Reason: {reason}",
            RelatedEntityId = leaveRequestId,
            SendEmail = true,
            Priority = NotificationPriority.High
        }, cancellationToken);
    }

    public async Task SendPendingApprovalReminderAsync(ObjectId approverId, ObjectId leaveRequestId, CancellationToken cancellationToken = default)
    {
        await SendNotificationAsync(new SendNotificationParams
        {
            RecipientId = approverId,
            Type = "PendingApprovalReminder",
            Title = "Pending Leave Approval",
            Message = "You have a pending leave request waiting for your approval.",
            RelatedEntityId = leaveRequestId,
            SendEmail = true,
            Priority = NotificationPriority.Normal
        }, cancellationToken);
    }

    public async Task SendUpcomingLeaveReminderAsync(ObjectId employeeId, ObjectId leaveRequestId, DateTime startDate, CancellationToken cancellationToken = default)
    {
        var daysUntil = (startDate.Date - DateTime.UtcNow.Date).Days;
        await SendNotificationAsync(new SendNotificationParams
        {
            RecipientId = employeeId,
            Type = "UpcomingLeaveReminder",
            Title = "Upcoming Leave Reminder",
            Message = $"Your approved leave starts in {daysUntil} days on {startDate:MMM dd, yyyy}.",
            RelatedEntityId = leaveRequestId,
            SendEmail = true,
            Priority = NotificationPriority.Normal
        }, cancellationToken);
    }
}
