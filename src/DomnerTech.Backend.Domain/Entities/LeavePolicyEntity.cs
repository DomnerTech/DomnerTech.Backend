using MongoDB.Bson;

namespace DomnerTech.Backend.Domain.Entities;

/// <summary>
/// Represents a company leave policy configuration.
/// </summary>
[MongoCollection("leave_policies")]
public sealed class LeavePolicyEntity : IBaseEntity, ITenantEntity, IAuditEntity, ISoftDeleteEntity
{
    [Sortable(alias: "id", order: 1)]
    public ObjectId Id { get; set; }
    public ObjectId CompanyId { get; set; }

    /// <summary>
    /// Gets or sets the policy name.
    /// </summary>
    public required string PolicyName { get; set; }

    /// <summary>
    /// Gets or sets the leave type this policy applies to.
    /// </summary>
    public ObjectId? LeaveTypeId { get; set; }

    /// <summary>
    /// Gets or sets the minimum notice period in days.
    /// </summary>
    public int MinimumNoticeDays { get; set; }

    /// <summary>
    /// Gets or sets the maximum consecutive days allowed per request.
    /// </summary>
    public int MaxConsecutiveDays { get; set; }

    /// <summary>
    /// Gets or sets whether weekends are included in leave calculation.
    /// </summary>
    public bool IncludeWeekends { get; set; }

    /// <summary>
    /// Gets or sets whether public holidays are included in leave calculation.
    /// </summary>
    public bool IncludePublicHolidays { get; set; }

    /// <summary>
    /// Gets or sets whether probationary employees can apply for this leave type.
    /// </summary>
    public bool AllowDuringProbation { get; set; }

    /// <summary>
    /// Gets or sets the probation period in months.
    /// </summary>
    public int? ProbationPeriodMonths { get; set; }

    /// <summary>
    /// Gets or sets whether negative balance is allowed.
    /// </summary>
    public bool AllowNegativeBalance { get; set; }

    /// <summary>
    /// Gets or sets the maximum negative balance allowed.
    /// </summary>
    public decimal? MaxNegativeBalance { get; set; }

    /// <summary>
    /// Gets or sets whether back-dated leave requests are allowed.
    /// </summary>
    public bool AllowBackdatedRequests { get; set; }

    /// <summary>
    /// Gets or sets the maximum backdated days allowed.
    /// </summary>
    public int? MaxBackdatedDays { get; set; }

    /// <summary>
    /// Gets or sets whether this policy is active.
    /// </summary>
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// Gets or sets the effective start date of the policy.
    /// </summary>
    public DateTime EffectiveFrom { get; set; }

    /// <summary>
    /// Gets or sets the effective end date of the policy.
    /// </summary>
    public DateTime? EffectiveTo { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public ObjectId? UpdatedBy { get; set; }
    public ObjectId? DeletedBy { get; set; }
    public bool IsDeleted { get; set; }
}
