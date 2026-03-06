using MongoDB.Bson;

namespace DomnerTech.Backend.Domain.Entities;

/// <summary>
/// Represents a notification in the system.
/// </summary>
[MongoCollection("notifications")]
public sealed class NotificationEntity : IBaseEntity, ITenantEntity
{
    [Sortable(alias: "id", order: 1)]
    public ObjectId Id { get; set; }
    public ObjectId CompanyId { get; set; }

    /// <summary>
    /// Gets or sets the recipient employee ID.
    /// </summary>
    public required ObjectId RecipientId { get; set; }

    /// <summary>
    /// Gets or sets the notification type.
    /// </summary>
    public required string Type { get; set; }

    /// <summary>
    /// Gets or sets the notification title.
    /// </summary>
    public required string Title { get; set; }

    /// <summary>
    /// Gets or sets the notification message.
    /// </summary>
    public required string Message { get; set; }

    /// <summary>
    /// Gets or sets related entity ID (e.g., LeaveRequestId).
    /// </summary>
    public ObjectId? RelatedEntityId { get; set; }

    /// <summary>
    /// Gets or sets whether the notification has been read.
    /// </summary>
    public bool IsRead { get; set; }

    /// <summary>
    /// Gets or sets the read date.
    /// </summary>
    public DateTime? ReadAt { get; set; }

    /// <summary>
    /// Gets or sets whether email was sent.
    /// </summary>
    public bool EmailSent { get; set; }

    /// <summary>
    /// Gets or sets whether SMS was sent.
    /// </summary>
    public bool SmsSent { get; set; }

    /// <summary>
    /// Gets or sets notification priority (Low, Normal, High).
    /// </summary>
    public string Priority { get; set; } = "Normal";

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
