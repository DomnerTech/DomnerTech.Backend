namespace DomnerTech.Backend.Application.DTOs.Leaves.TeamLeave;

/// <summary>
/// DTO for team leave calendar entry.
/// </summary>
public sealed record TeamLeaveCalendarDto
{
    public required string EmployeeId { get; set; }
    public required string EmployeeName { get; set; }
    public required string Department { get; set; }
    public required string LeaveTypeName { get; set; }
    public required DateTime StartDate { get; set; }
    public required DateTime EndDate { get; set; }
    public required decimal TotalDays { get; set; }
    public required string Status { get; set; }
}

/// <summary>
/// DTO for team leave conflict detection.
/// </summary>
public sealed record TeamLeaveConflictDto
{
    public required DateTime Date { get; set; }
    public required int EmployeesOnLeave { get; set; }
    public required int MaxAllowed { get; set; }
    public bool HasConflict => EmployeesOnLeave > MaxAllowed;
    public List<string> EmployeeNames { get; set; } = [];
}

/// <summary>
/// DTO for team leave statistics.
/// </summary>
public sealed record TeamLeaveStatsDto
{
    public required string Department { get; set; }
    public required int TotalEmployees { get; set; }
    public required int EmployeesOnLeaveToday { get; set; }
    public required int PendingRequests { get; set; }
    public required decimal AverageLeaveUsage { get; set; }
}

/// <summary>
/// Request DTO for checking team leave conflicts.
/// </summary>
public sealed record CheckTeamLeaveConflictReqDto
{
    public required string Department { get; set; }
    public required DateTime StartDate { get; set; }
    public required DateTime EndDate { get; set; }
    public int MaxEmployeesOnLeave { get; set; } = 2;
    public string? ExcludeEmployeeId { get; set; }
}
