using MongoDB.Bson;

namespace DomnerTech.Backend.Domain.Entities;

/// <summary>
/// Represents an audit log entry tracking system actions.
/// </summary>
[MongoCollection("audit_logs")]
public sealed class AuditLogEntity : IBaseEntity, ITenantEntity
{
    [Sortable(alias: "id", order: 1)]
    public ObjectId Id { get; set; }
    public ObjectId CompanyId { get; set; }

    /// <summary>
    /// Gets or sets the user who performed the action.
    /// </summary>
    public required ObjectId UserId { get; set; }

    /// <summary>
    /// Gets or sets the user's name at the time of action.
    /// </summary>
    public required string UserName { get; set; }

    /// <summary>
    /// Gets or sets the action type.
    /// </summary>
    public required string Action { get; set; }

    /// <summary>
    /// Gets or sets the entity type affected.
    /// </summary>
    public required string EntityType { get; set; }

    /// <summary>
    /// Gets or sets the entity ID affected.
    /// </summary>
    public ObjectId? EntityId { get; set; }

    /// <summary>
    /// Gets or sets the action description.
    /// </summary>
    public required string Description { get; set; }

    /// <summary>
    /// Gets or sets the old values (JSON).
    /// </summary>
    public string? OldValues { get; set; }

    /// <summary>
    /// Gets or sets the new values (JSON).
    /// </summary>
    public string? NewValues { get; set; }

    /// <summary>
    /// Gets or sets the IP address.
    /// </summary>
    public string? IpAddress { get; set; }

    /// <summary>
    /// Gets or sets the user agent.
    /// </summary>
    public string? UserAgent { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
