using DomnerTech.Backend.Domain.ValueObjects;
using MongoDB.Bson;

namespace DomnerTech.Backend.Domain.Entities;

/// <summary>
/// Represents an employee's leave balance for a specific leave type.
/// </summary>
[MongoCollection("leave_balances")]
public sealed class LeaveBalanceEntity : IBaseEntity, ITenantEntity, IAuditEntity
{
    [Sortable(alias: "id", order: 1)]
    public ObjectId Id { get; set; }
    public ObjectId CompanyId { get; set; }

    /// <summary>
    /// Gets or sets the employee ID.
    /// </summary>
    public required ObjectId EmployeeId { get; set; }

    /// <summary>
    /// Gets or sets the leave type ID.
    /// </summary>
    public required ObjectId LeaveTypeId { get; set; }

    /// <summary>
    /// Gets or sets the year this balance applies to.
    /// </summary>
    [Sortable(alias: "year", order: 2)]
    public required int Year { get; set; }

    /// <summary>
    /// Gets or sets the leave allowance details.
    /// </summary>
    public required LeaveAllowanceValueObject Allowance { get; set; }

    /// <summary>
    /// Gets or sets the last accrual date (for accrual-based leave).
    /// </summary>
    public DateTime? LastAccrualDate { get; set; }

    /// <summary>
    /// Gets or sets the expiry date for carried forward leave.
    /// </summary>
    public DateTime? CarryForwardExpiryDate { get; set; }

    /// <summary>
    /// Gets or sets whether this balance is active.
    /// </summary>
    public bool IsActive { get; set; } = true;

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public ObjectId? UpdatedBy { get; set; }
    public ObjectId? DeletedBy { get; set; }
}
