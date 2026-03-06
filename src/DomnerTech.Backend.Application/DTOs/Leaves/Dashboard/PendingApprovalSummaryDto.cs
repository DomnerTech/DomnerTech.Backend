namespace DomnerTech.Backend.Application.DTOs.Leaves.Dashboard;

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
