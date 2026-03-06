using MongoDB.Bson;

namespace DomnerTech.Backend.Domain.Entities;

/// <summary>
/// Represents a blackout date period where leave requests are not allowed.
/// </summary>
[MongoCollection("leaveBlackoutDates")]
public sealed class LeaveBlackoutDateEntity : IBaseEntity, ITenantEntity, IAuditEntity, ISoftDeleteEntity
{
    [Sortable(alias: "id", order: 1)]
    public ObjectId Id { get; set; }
    public ObjectId CompanyId { get; set; }

    /// <summary>
    /// Gets or sets the blackout period name.
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    /// Gets or sets the blackout period description.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Gets or sets the start date of the blackout period.
    /// </summary>
    public required DateTime StartDate { get; set; }

    /// <summary>
    /// Gets or sets the end date of the blackout period.
    /// </summary>
    public required DateTime EndDate { get; set; }

    /// <summary>
    /// Gets or sets the departments affected (null means all departments).
    /// </summary>
    public List<string>? Departments { get; set; }

    /// <summary>
    /// Gets or sets whether this blackout date is active.
    /// </summary>
    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public ObjectId? CreatedBy { get; set; }
    public ObjectId? UpdatedBy { get; set; }
    public ObjectId? DeletedBy { get; set; }
    public bool IsDeleted { get; set; }
}
