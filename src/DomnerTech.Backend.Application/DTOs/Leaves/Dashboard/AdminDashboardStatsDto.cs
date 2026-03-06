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