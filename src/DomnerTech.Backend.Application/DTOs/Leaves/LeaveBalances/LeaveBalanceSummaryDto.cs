namespace DomnerTech.Backend.Application.DTOs.Leaves.LeaveBalances;

/// <summary>
/// DTO for leave balance summary.
/// </summary>
public sealed record LeaveBalanceSummaryDto
{
    public required string LeaveTypeName { get; set; }
    public required string LeaveTypeCode { get; set; }
    public required decimal TotalAllowance { get; set; }
    public required decimal UsedDays { get; set; }
    public required decimal RemainingDays { get; set; }
    public decimal CarriedForwardDays { get; set; }
}
