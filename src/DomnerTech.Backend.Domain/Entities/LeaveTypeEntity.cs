using DomnerTech.Backend.Domain.Enums;
using MongoDB.Bson;

namespace DomnerTech.Backend.Domain.Entities;

/// <summary>
/// Represents a leave type configuration in the system.
/// </summary>
[MongoCollection("leave_types")]
public sealed class LeaveTypeEntity : IBaseEntity, ITenantEntity, IAuditEntity, ISoftDeleteEntity
{
    [Sortable(alias: "id", order: 1)]
    public ObjectId Id { get; set; }
    public ObjectId CompanyId { get; set; }

    /// <summary>
    /// Gets or sets the name of the leave type (e.g., Annual Leave, Sick Leave).
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    /// Gets or sets the description of the leave type.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Gets or sets the code/identifier for the leave type.
    /// </summary>
    public required string Code { get; set; }

    /// <summary>
    /// Gets or sets the yearly allowance in days for this leave type.
    /// </summary>
    public decimal YearlyAllowance { get; set; }

    /// <summary>
    /// Gets or sets whether this leave type uses accrual-based system.
    /// </summary>
    public bool IsAccrualBased { get; set; }

    /// <summary>
    /// Gets or sets the monthly accrual amount (if accrual-based).
    /// </summary>
    public decimal? MonthlyAccrualDays { get; set; }

    /// <summary>
    /// Gets or sets the maximum carry forward days allowed.
    /// </summary>
    public decimal MaxCarryForwardDays { get; set; }

    /// <summary>
    /// Gets or sets whether carried forward leave expires.
    /// </summary>
    public bool CarryForwardExpires { get; set; }

    /// <summary>
    /// Gets or sets the carry forward expiry date (month and day).
    /// </summary>
    public DateTime? CarryForwardExpiryDate { get; set; }

    /// <summary>
    /// Gets or sets whether supporting documents are required for this leave type.
    /// </summary>
    public bool RequiresDocument { get; set; }

    /// <summary>
    /// Gets or sets whether this leave type is paid.
    /// </summary>
    public bool IsPaid { get; set; }

    /// <summary>
    /// Gets or sets whether this leave type is active.
    /// </summary>
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// Gets or sets the display order for sorting.
    /// </summary>
    public int DisplayOrder { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public ObjectId? UpdatedBy { get; set; }
    public ObjectId? DeletedBy { get; set; }
    public bool IsDeleted { get; set; }
}
