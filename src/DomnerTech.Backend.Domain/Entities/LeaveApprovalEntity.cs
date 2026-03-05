using DomnerTech.Backend.Domain.Enums;
using MongoDB.Bson;

namespace DomnerTech.Backend.Domain.Entities;

/// <summary>
/// Represents an approval step in the leave request workflow.
/// </summary>
[MongoCollection("leave_approvals")]
public sealed class LeaveApprovalEntity : IBaseEntity, ITenantEntity, IAuditEntity
{
    [Sortable(alias: "id", order: 1)]
    public ObjectId Id { get; set; }
    public ObjectId CompanyId { get; set; }

    /// <summary>
    /// Gets or sets the leave request ID.
    /// </summary>
    public required ObjectId LeaveRequestId { get; set; }

    /// <summary>
    /// Gets or sets the approval level.
    /// </summary>
    public required ApprovalLevel Level { get; set; }

    /// <summary>
    /// Gets or sets the approver ID.
    /// </summary>
    public required ObjectId ApproverId { get; set; }

    /// <summary>
    /// Gets or sets the approval status.
    /// </summary>
    [Sortable(alias: "status", order: 2)]
    public LeaveRequestStatus Status { get; set; } = LeaveRequestStatus.Pending;

    /// <summary>
    /// Gets or sets the comments from the approver.
    /// </summary>
    public string? Comments { get; set; }

    /// <summary>
    /// Gets or sets the action date (approved/rejected date).
    /// </summary>
    [Sortable(alias: "actionDate", order: 3)]
    public DateTime? ActionDate { get; set; }

    /// <summary>
    /// Gets or sets the sequence order in the approval workflow.
    /// </summary>
    public int SequenceOrder { get; set; }

    /// <summary>
    /// Gets or sets whether this is the final approval step.
    /// </summary>
    public bool IsFinalApproval { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public ObjectId? UpdatedBy { get; set; }
    public ObjectId? DeletedBy { get; set; }
}
