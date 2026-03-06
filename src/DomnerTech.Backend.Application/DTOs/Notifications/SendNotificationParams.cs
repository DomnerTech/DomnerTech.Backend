using DomnerTech.Backend.Application.Constants;
using MongoDB.Bson;

namespace DomnerTech.Backend.Application.DTOs.Notifications;

public class SendNotificationParams
{
    public ObjectId RecipientId { get; set; }
    public required string Type { get; set; }
    public required string Title { get; set; }
    public required string Message { get; set; }
    public ObjectId? RelatedEntityId { get; set; }
    public bool SendEmail { get; set; }
    public bool SendSms { get; set; }
    public string Priority { get; set; } = NotificationPriority.Normal;
}