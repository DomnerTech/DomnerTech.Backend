namespace DomnerTech.Backend.Application.DTOs.Leaves.LeavePolicies;

/// <summary>
/// DTO for updating a leave policy.
/// </summary>
public sealed record UpdateLeavePolicyReqDto
{
    /// <summary>
    /// Gets or sets the policy ID.
    /// </summary>
    public required string Id { get; set; }

    /// <summary>
    /// Gets or sets the policy name.
    /// </summary>
    public required string Name { get; set; }

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
    /// Gets or sets whether probationary employees can apply.
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
    /// Gets or sets whether back-dated requests are allowed.
    /// </summary>
    public bool AllowBackdatedRequests { get; set; }

    /// <summary>
    /// Gets or sets the maximum backdated days allowed.
    /// </summary>
    public int? MaxBackdatedDays { get; set; }

    /// <summary>
    /// Gets or sets whether this policy is active.
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// Gets or sets the effective end date.
    /// </summary>
    public DateTime? EffectiveTo { get; set; }
}
