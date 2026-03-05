namespace DomnerTech.Backend.Domain.Enums;

/// <summary>
/// Represents the type of holiday.
/// </summary>
public enum HolidayType
{
    /// <summary>
    /// Public holiday recognized by the country.
    /// </summary>
    Public = 0,

    /// <summary>
    /// Company-specific holiday.
    /// </summary>
    Company = 1,

    /// <summary>
    /// Optional holiday that employees can choose to take.
    /// </summary>
    Optional = 2
}
