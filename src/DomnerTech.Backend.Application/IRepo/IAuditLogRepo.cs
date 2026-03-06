using DomnerTech.Backend.Domain.Entities;
using MongoDB.Bson;

namespace DomnerTech.Backend.Application.IRepo;

/// <summary>
/// Repository interface for audit log operations.
/// </summary>
public interface IAuditLogRepo : IBaseRepo
{
    Task<ObjectId> CreateAsync(AuditLogEntity entity, CancellationToken cancellationToken = default);
    Task<List<AuditLogEntity>> GetByEntityAsync(string entityType, ObjectId entityId, CancellationToken cancellationToken = default);
    Task<List<AuditLogEntity>> GetByUserAsync(ObjectId userId, int limit = 100, CancellationToken cancellationToken = default);
    Task<List<AuditLogEntity>> GetByActionAsync(string action, int limit = 100, CancellationToken cancellationToken = default);
    Task<List<AuditLogEntity>> GetRecentAsync(int limit = 100, CancellationToken cancellationToken = default);
}
