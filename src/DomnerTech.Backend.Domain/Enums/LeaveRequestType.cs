namespace DomnerTech.Backend.Domain.Enums;

/// <summary>
/// Represents the type of leave request based on duration.
/// </summary>
public enum LeaveRequestType
{
    /// <summary>
    /// Full day leave.
    /// </summary>
    FullDay = 0,

    /// <summary>
    /// Half day leave in the morning.
    /// </summary>
    HalfDayAM = 1,

    /// <summary>
    /// Half day leave in the afternoon.
    /// </summary>
    HalfDayPM = 2
}
