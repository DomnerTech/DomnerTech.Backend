namespace DomnerTech.Backend.Application.DTOs.Notifications;

/// <summary>
/// DTO representing a notification.
/// </summary>
public sealed record NotificationDto
{
    public required string Id { get; set; }
    public required string Type { get; set; }
    public required string Title { get; set; }
    public required string Message { get; set; }
    public string? RelatedEntityId { get; set; }
    public bool IsRead { get; set; }
    public DateTime? ReadAt { get; set; }
    public required string Priority { get; set; }
    public DateTime CreatedAt { get; set; }
}

/// <summary>
/// DTO for creating a notification.
/// </summary>
public sealed record CreateNotificationReqDto
{
    public required string RecipientId { get; set; }
    public required string Type { get; set; }
    public required string Title { get; set; }
    public required string Message { get; set; }
    public string? RelatedEntityId { get; set; }
    public string Priority { get; set; } = "Normal";
    public bool SendEmail { get; set; }
    public bool SendSms { get; set; }
}

/// <summary>
/// DTO for notification preferences.
/// </summary>
public sealed record NotificationPreferencesDto
{
    public bool EmailNotifications { get; set; } = true;
    public bool SmsNotifications { get; set; } = false;
    public bool InAppNotifications { get; set; } = true;
    public bool LeaveApprovalNotifications { get; set; } = true;
    public bool LeaveRejectionNotifications { get; set; } = true;
    public bool LeaveReminderNotifications { get; set; } = true;
}
