namespace DomnerTech.Backend.Application.DTOs.Notifications;

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