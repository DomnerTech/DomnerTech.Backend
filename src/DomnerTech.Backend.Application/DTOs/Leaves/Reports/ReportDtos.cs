namespace DomnerTech.Backend.Application.DTOs.Leaves.Reports;

/// <summary>
/// DTO for leave usage report.
/// </summary>
public sealed record LeaveUsageReportDto
{
    public required string EmployeeId { get; set; }
    public required string EmployeeName { get; set; }
    public required string Department { get; set; }
    public required string LeaveTypeName { get; set; }
    public required decimal TotalAllowance { get; set; }
    public required decimal UsedDays { get; set; }
    public required decimal RemainingDays { get; set; }
    public required decimal UsagePercentage { get; set; }
}

/// <summary>
/// DTO for department leave statistics.
/// </summary>
public sealed record DepartmentLeaveStatsDto
{
    public required string Department { get; set; }
    public required int TotalEmployees { get; set; }
    public required decimal TotalLeaveDays { get; set; }
    public required decimal AverageLeaveDays { get; set; }
    public required int TotalRequests { get; set; }
    public required int ApprovedRequests { get; set; }
    public required int PendingRequests { get; set; }
    public required int RejectedRequests { get; set; }
}

/// <summary>
/// DTO for leave trend analysis.
/// </summary>
public sealed record LeaveTrendDto
{
    public required string Month { get; set; }
    public required int Year { get; set; }
    public required int TotalRequests { get; set; }
    public required decimal TotalDays { get; set; }
    public required int ApprovedCount { get; set; }
    public required int RejectedCount { get; set; }
}

/// <summary>
/// DTO for employee leave summary.
/// </summary>
public sealed record EmployeeLeaveSummaryDto
{
    public required string EmployeeId { get; set; }
    public required string EmployeeName { get; set; }
    public required List<LeaveTypeSummary> LeaveTypes { get; set; }
    public required int TotalRequestsThisYear { get; set; }
    public required decimal TotalDaysTaken { get; set; }
}

public sealed record LeaveTypeSummary
{
    public required string LeaveTypeName { get; set; }
    public required decimal Allowance { get; set; }
    public required decimal Used { get; set; }
    public required decimal Remaining { get; set; }
}
