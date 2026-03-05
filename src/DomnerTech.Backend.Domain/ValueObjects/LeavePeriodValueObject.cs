namespace DomnerTech.Backend.Domain.ValueObjects;

/// <summary>
/// Represents a leave period with start and end dates.
/// </summary>
public sealed record LeavePeriodValueObject
{
    /// <summary>
    /// Gets or sets the start date of the leave period.
    /// </summary>
    public required DateTime StartDate { get; set; }

    /// <summary>
    /// Gets or sets the end date of the leave period.
    /// </summary>
    public required DateTime EndDate { get; set; }

    /// <summary>
    /// Calculates the number of days in the leave period (inclusive).
    /// </summary>
    public int TotalDays => (EndDate.Date - StartDate.Date).Days + 1;
}
