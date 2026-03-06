using DomnerTech.Backend.Domain.Entities;
using MongoDB.Bson;

namespace DomnerTech.Backend.Application.IRepo;

/// <summary>
/// Repository interface for notification operations.
/// </summary>
public interface INotificationRepo : IBaseRepo
{
    Task<ObjectId> CreateAsync(NotificationEntity entity, CancellationToken cancellationToken = default);
    Task UpdateAsync(NotificationEntity entity, CancellationToken cancellationToken = default);
    Task<NotificationEntity?> GetByIdAsync(ObjectId id, CancellationToken cancellationToken = default);
    Task<List<NotificationEntity>> GetByRecipientAsync(ObjectId recipientId, int limit = 50, CancellationToken cancellationToken = default);
    Task<List<NotificationEntity>> GetUnreadByRecipientAsync(ObjectId recipientId, CancellationToken cancellationToken = default);
    Task<int> GetUnreadCountAsync(ObjectId recipientId, CancellationToken cancellationToken = default);
    Task MarkAsReadAsync(ObjectId id, CancellationToken cancellationToken = default);
    Task MarkAllAsReadAsync(ObjectId recipientId, CancellationToken cancellationToken = default);
}
