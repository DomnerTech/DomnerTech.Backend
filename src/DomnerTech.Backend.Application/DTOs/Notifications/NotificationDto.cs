using DomnerTech.Backend.Domain.Entities;

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

public static class NotificationExtensions
{
    public static NotificationDto ToDto(this NotificationEntity entity)
    {
        return new NotificationDto
        {
            Id = entity.Id.ToString(),
            Type = entity.Type,
            Title = entity.Title,
            Message = entity.Message,
            RelatedEntityId = entity.RelatedEntityId?.ToString(),
            IsRead = entity.IsRead,
            ReadAt = entity.ReadAt,
            Priority = entity.Priority,
            CreatedAt = entity.CreatedAt
        };
    }
}