namespace DomnerTech.Backend.Application.DTOs.Leaves.Dashboard;

/// <summary>
/// DTO for admin dashboard statistics.
/// </summary>
public sealed record AdminDashboardStatsDto
{
    public required int TotalEmployees { get; set; }
    public required int EmployeesOnLeaveToday { get; set; }
    public required int PendingApprovals { get; set; }
    public required int UpcomingLeaves { get; set; }
    public required decimal TotalLeaveDaysThisMonth { get; set; }
    public required decimal AverageLeavePerEmployee { get; set; }
    public required int TotalRequestsThisMonth { get; set; }
    public required int ApprovedThisMonth { get; set; }
    public required int RejectedThisMonth { get; set; }
}

/// <summary>
/// DTO for employees currently on leave.
/// </summary>
public sealed record EmployeeOnLeaveDto
{
    public required string EmployeeId { get; set; }
    public required string EmployeeName { get; set; }
    public required string Department { get; set; }
    public required string LeaveTypeName { get; set; }
    public required DateTime StartDate { get; set; }
    public required DateTime EndDate { get; set; }
    public required decimal TotalDays { get; set; }
}

/// <summary>
/// DTO for upcoming leave.
/// </summary>
public sealed record UpcomingLeaveDto
{
    public required string EmployeeId { get; set; }
    public required string EmployeeName { get; set; }
    public required string Department { get; set; }
    public required string LeaveTypeName { get; set; }
    public required DateTime StartDate { get; set; }
    public required DateTime EndDate { get; set; }
    public required decimal TotalDays { get; set; }
    public required int DaysUntilStart { get; set; }
}

/// <summary>
/// DTO for pending approval summary.
/// </summary>
public sealed record PendingApprovalSummaryDto
{
    public required string LeaveRequestId { get; set; }
    public required string EmployeeId { get; set; }
    public required string EmployeeName { get; set; }
    public required string Department { get; set; }
    public required string LeaveTypeName { get; set; }
    public required DateTime StartDate { get; set; }
    public required DateTime EndDate { get; set; }
    public required decimal TotalDays { get; set; }
    public required DateTime SubmittedAt { get; set; }
    public required int PendingDays { get; set; }
}
