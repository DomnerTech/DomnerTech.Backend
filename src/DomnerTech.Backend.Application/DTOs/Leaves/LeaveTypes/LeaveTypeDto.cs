using DomnerTech.Backend.Domain.Entities;

namespace DomnerTech.Backend.Application.DTOs.Leaves.LeaveTypes;

/// <summary>
/// DTO representing a leave type.
/// </summary>
public sealed record LeaveTypeDto : IBaseDto
{
    /// <summary>
    /// Gets or sets the leave type ID.
    /// </summary>
    public required string Id { get; set; }

    /// <summary>
    /// Gets or sets the name of the leave type.
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
    public bool IsPaid { get; set; }

    /// <summary>
    /// Gets or sets whether this leave type is active.
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// Gets or sets the display order.
    /// </summary>
    public int DisplayOrder { get; set; }

    /// <summary>
    /// Gets or sets the creation date.
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Gets or sets the last update date.
    /// </summary>
    public DateTime UpdatedAt { get; set; }
}
public static class LeaveTypeExtensions
{
    /// <summary>
    /// Converts a <see cref="LeaveTypeEntity"/> to a <see cref="LeaveTypeDto"/>.
    /// </summary>
    public static LeaveTypeDto ToDto(this LeaveTypeEntity entity)
    {
        return new LeaveTypeDto
        {
            Id = entity.Id.ToString(),
            Name = entity.Name,
            Description = entity.Description,
            Code = entity.Code,
            YearlyAllowance = entity.YearlyAllowance,
            IsAccrualBased = entity.IsAccrualBased,
            MonthlyAccrualDays = entity.MonthlyAccrualDays,
            MaxCarryForwardDays = entity.MaxCarryForwardDays,
            CarryForwardExpires = entity.CarryForwardExpires,
            CarryForwardExpiryDate = entity.CarryForwardExpiryDate,
            RequiresDocument = entity.RequiresDocument,
            IsPaid = entity.IsPaid,
            IsActive = entity.IsActive,
            DisplayOrder = entity.DisplayOrder,
            CreatedAt = entity.CreatedAt,
            UpdatedAt = entity.UpdatedAt
        };
    }
}