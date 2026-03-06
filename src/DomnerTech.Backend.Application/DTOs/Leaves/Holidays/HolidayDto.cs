using DomnerTech.Backend.Application.DTOs;
using DomnerTech.Backend.Domain.Entities;
using DomnerTech.Backend.Domain.Enums;

namespace DomnerTech.Backend.Application.DTOs.Leaves.Holidays;

/// <summary>
/// DTO representing a holiday.
/// </summary>
public sealed record HolidayDto : IBaseDto
{
    /// <summary>
    /// Gets or sets the holiday ID.
    /// </summary>
    public required string Id { get; set; }

    /// <summary>
    /// Gets or sets the name of the holiday.
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    /// Gets or sets the description of the holiday.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Gets or sets the holiday date.
    /// </summary>
    public required DateTime Date { get; set; }

    /// <summary>
    /// Gets or sets the type of holiday.
    /// </summary>
    public HolidayType Type { get; set; }

    /// <summary>
    /// Gets or sets whether this holiday is recurring annually.
    /// </summary>
    public bool IsRecurring { get; set; }

    /// <summary>
    /// Gets or sets the country code.
    /// </summary>
    public string? CountryCode { get; set; }

    /// <summary>
    /// Gets or sets the region/state.
    /// </summary>
    public string? Region { get; set; }

    /// <summary>
    /// Gets or sets whether this holiday is active.
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// Gets or sets the creation date.
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Gets or sets the last update date.
    /// </summary>
    public DateTime UpdatedAt { get; set; }
}

public static class HolidayDtoExtensions
{
    public static HolidayDto ToDto(this HolidayEntity entity)
    {
        return new HolidayDto
        {
            Id = entity.Id.ToString(),
            Name = entity.Name,
            Description = entity.Description,
            Date = entity.Date,
            Type = entity.Type,
            IsRecurring = entity.IsRecurring,
            CountryCode = entity.CountryCode,
            Region = entity.Region,
            IsActive = entity.IsActive,
            CreatedAt = entity.CreatedAt,
            UpdatedAt = entity.UpdatedAt
        };
    }
}