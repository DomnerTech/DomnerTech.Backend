namespace DomnerTech.Backend.Domain.ValueObjects;

/// <summary>
/// Represents leave allowance details including total, used, and remaining days.
/// </summary>
public sealed record LeaveAllowanceValueObject
{
    /// <summary>
    /// Gets or sets the total annual allowance in days.
    /// </summary>
    public required decimal TotalAllowance { get; set; }

    /// <summary>
    /// Gets or sets the used leave days.
    /// </summary>
    public required decimal UsedDays { get; set; }

    /// <summary>
    /// Gets or sets the remaining leave days.
    /// </summary>
    public decimal RemainingDays => TotalAllowance - UsedDays;

    /// <summary>
    /// Gets or sets the carried forward days from previous year.
    /// </summary>
    public decimal CarriedForwardDays { get; set; }
}
