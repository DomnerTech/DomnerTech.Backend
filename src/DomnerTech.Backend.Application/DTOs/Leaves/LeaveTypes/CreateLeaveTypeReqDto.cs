using DomnerTech.Backend.Application.DTOs;

namespace DomnerTech.Backend.Application.DTOs.Leaves.LeaveTypes;

/// <summary>
/// DTO for creating a new leave type.
/// </summary>
public sealed record CreateLeaveTypeReqDto
{
    /// <summary>
    /// Gets or sets the name of the leave type.
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    /// Gets or sets the description of the leave type.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Gets or sets the unique code for the leave type.
    /// </summary>
    public required string Code { get; set; }

    /// <summary>
    /// Gets or sets the yearly allowance in days.
    /// </summary>
    public decimal YearlyAllowance { get; set; }

    /// <summary>
    /// Gets or sets whether this leave type uses accrual-based system.
    /// </summary>
    public bool IsAccrualBased { get; set; }

    /// <summary>
    /// Gets or sets the monthly accrual amount.
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
    /// Gets or sets the carry forward expiry date.
    /// </summary>
    public DateTime? CarryForwardExpiryDate { get; set; }

    /// <summary>
    /// Gets or sets whether supporting documents are required.
    /// </summary>
    public bool RequiresDocument { get; set; }

    /// <summary>
    /// Gets or sets whether this leave type is paid.
    /// </summary>
    public bool IsPaid { get; set; } = true;

    /// <summary>
    /// Gets or sets the display order.
    /// </summary>
    public int DisplayOrder { get; set; }
}
