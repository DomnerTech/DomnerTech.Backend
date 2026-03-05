namespace DomnerTech.Backend.Application.DTOs.Leaves.Holidays;

/// <summary>
/// DTO for bulk creating holidays.
/// </summary>
public sealed record BulkCreateHolidaysReqDto
{
    /// <summary>
    /// Gets or sets the list of holidays to create.
    /// </summary>
    public required List<CreateHolidayReqDto> Holidays { get; set; }
}
