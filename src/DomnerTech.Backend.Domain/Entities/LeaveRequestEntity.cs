using DomnerTech.Backend.Domain.Enums;
using DomnerTech.Backend.Domain.ValueObjects;
using MongoDB.Bson;

namespace DomnerTech.Backend.Domain.Entities;

/// <summary>
/// Represents an employee leave request.
/// </summary>
[MongoCollection("leave_requests")]
public sealed class LeaveRequestEntity : IBaseEntity, ITenantEntity, IAuditEntity, ISoftDeleteEntity
{
    [Sortable(alias: "id", order: 1)]
    public ObjectId Id { get; set; }
    public ObjectId CompanyId { get; set; }

    /// <summary>
    /// Gets or sets the employee ID who requested the leave.
    /// </summary>
    public required ObjectId EmployeeId { get; set; }

    /// <summary>
    /// Gets or sets the leave type ID.
    /// </summary>
    public required ObjectId LeaveTypeId { get; set; }

    /// <summary>
    /// Gets or sets the leave period.
    /// </summary>
    public required LeavePeriodValueObject Period { get; set; }

    /// <summary>
    /// Gets or sets the type of leave request (full day, half day).
    /// </summary>
    public LeaveRequestType RequestType { get; set; } = LeaveRequestType.FullDay;

    /// <summary>
    /// Gets or sets the total number of days requested (accounting for half days).
    /// </summary>
    public required decimal TotalDays { get; set; }

    /// <summary>
    /// Gets or sets the reason for the leave request.
    /// </summary>
    public string? Reason { get; set; }

    /// <summary>
    /// Gets or sets additional notes from the employee.
    /// </summary>
    public string? Notes { get; set; }

    /// <summary>
    /// Gets or sets the document URLs for supporting documents.
    /// </summary>
    public List<string>? DocumentUrls { get; set; }

    /// <summary>
    /// Gets or sets the current status of the leave request.
    /// </summary>
    [Sortable(alias: "status", order: 2)]
    public LeaveRequestStatus Status { get; set; } = LeaveRequestStatus.Pending;

    /// <summary>
    /// Gets or sets the submission date.
    /// </summary>
    [Sortable(alias: "submittedAt", order: 3)]
    public DateTime SubmittedAt { get; set; }

    /// <summary>
    /// Gets or sets the current approval level (if multi-level approval).
    /// </summary>
    public ApprovalLevel? CurrentApprovalLevel { get; set; }

    /// <summary>
    /// Gets or sets the approver ID (final approver if multi-level).
    /// </summary>
    public ObjectId? ApprovedBy { get; set; }

    /// <summary>
    /// Gets or sets the approval date.
    /// </summary>
    public DateTime? ApprovedAt { get; set; }

    /// <summary>
    /// Gets or sets the rejection reason.
    /// </summary>
    public string? RejectionReason { get; set; }

    /// <summary>
    /// Gets or sets the cancellation reason.
    /// </summary>
    public string? CancellationReason { get; set; }

    /// <summary>
    /// Gets or sets the cancellation date.
    /// </summary>
    public DateTime? CancelledAt { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public ObjectId? UpdatedBy { get; set; }
    public ObjectId? DeletedBy { get; set; }
    public bool IsDeleted { get; set; }
}
