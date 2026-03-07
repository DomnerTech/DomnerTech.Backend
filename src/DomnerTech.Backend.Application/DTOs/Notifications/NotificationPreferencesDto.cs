namespace DomnerTech.Backend.Application.DTOs.Notifications;

/// <summary>
/// DTO for notification preferences.
/// </summary>
public sealed record NotificationPreferencesDto
{
    public bool EmailNotifications { get; set; } = true;
    public bool SmsNotifications { get; set; }
    public bool InAppNotifications { get; set; } = true;
    public bool LeaveApprovalNotifications { get; set; } = true;
    public bool LeaveRejectionNotifications { get; set; } = true;
    public bool LeaveReminderNotifications { get; set; } = true;
}
