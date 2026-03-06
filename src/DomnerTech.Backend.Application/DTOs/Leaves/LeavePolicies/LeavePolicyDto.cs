using DomnerTech.Backend.Domain.Entities;

namespace DomnerTech.Backend.Application.DTOs.Leaves.LeavePolicies;

/// <summary>
/// DTO representing a leave policy.
/// </summary>
public sealed record LeavePolicyDto : IBaseDto
{
    /// <summary>
    /// Gets or sets the policy ID.
    /// </summary>
    public required string Id { get; set; }

    /// <summary>
    /// Gets or sets the policy name.
    /// </summary>
    public required string PolicyName { get; set; }

    /// <summary>
    /// Gets or sets the leave type ID.
    /// </summary>
    public string? LeaveTypeId { get; set; }

    /// <summary>
    /// Gets or sets the minimum notice period in days.
    /// </summary>
    public int MinimumNoticeDays { get; set; }

    /// <summary>
    /// Gets or sets the maximum consecutive days allowed per request.
    /// </summary>
    public int MaxConsecutiveDays { get; set; }

    /// <summary>
    /// Gets or sets whether weekends are included.
    /// </summary>
    public bool IncludeWeekends { get; set; }

    /// <summary>
    /// Gets or sets whether public holidays are included.
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
    /// Gets or sets the maximum negative balance.
    /// </summary>
    public decimal? MaxNegativeBalance { get; set; }

    /// <summary>
    /// Gets or sets whether back-dated requests are allowed.
    /// </summary>
    public bool AllowBackdatedRequests { get; set; }

    /// <summary>
    /// Gets or sets the maximum backdated days.
    /// </summary>
    public int? MaxBackdatedDays { get; set; }

    /// <summary>
    /// Gets or sets whether this policy is active.
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// Gets or sets the effective start date.
    /// </summary>
    public DateTime EffectiveFrom { get; set; }

    /// <summary>
    /// Gets or sets the effective end date.
    /// </summary>
    public DateTime? EffectiveTo { get; set; }

    /// <summary>
    /// Gets or sets the creation date.
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Gets or sets the last update date.
    /// </summary>
    public DateTime UpdatedAt { get; set; }
}

public static class LeavePolicyExtensions
{
    /// <summary>
    /// Converts a <see cref="LeavePolicyEntity"/> to a <see cref="LeavePolicyDto"/>.
    /// </summary>
    /// <param name="entity">The leave policy entity.</param>
    /// <returns>The corresponding leave policy DTO.</returns>
    public static LeavePolicyDto ToDto(this LeavePolicyEntity entity)
    {
        return new LeavePolicyDto
        {
            Id = entity.Id.ToString(),
            PolicyName = entity.PolicyName,
            LeaveTypeId = entity.LeaveTypeId?.ToString(),
            MinimumNoticeDays = entity.MinimumNoticeDays,
            MaxConsecutiveDays = entity.MaxConsecutiveDays,
            IncludeWeekends = entity.IncludeWeekends,
            IncludePublicHolidays = entity.IncludePublicHolidays,
            AllowDuringProbation = entity.AllowDuringProbation,
            ProbationPeriodMonths = entity.ProbationPeriodMonths,
            AllowNegativeBalance = entity.AllowNegativeBalance,
            MaxNegativeBalance = entity.MaxNegativeBalance,
            AllowBackdatedRequests = entity.AllowBackdatedRequests,
            MaxBackdatedDays = entity.MaxBackdatedDays,
            IsActive = entity.IsActive,
            EffectiveFrom = entity.EffectiveFrom,
            EffectiveTo = entity.EffectiveTo,
            CreatedAt = entity.CreatedAt,
            UpdatedAt = entity.UpdatedAt
        };
    }
}