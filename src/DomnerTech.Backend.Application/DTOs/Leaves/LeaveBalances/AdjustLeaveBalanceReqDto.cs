namespace DomnerTech.Backend.Application.DTOs.Leaves.LeaveBalances;

/// <summary>
/// DTO for adjusting leave balance.
/// </summary>
public sealed record AdjustLeaveBalanceReqDto
{
    public required string EmployeeId { get; set; }
    public required string LeaveTypeId { get; set; }
    public required int Year { get; set; }
    public required decimal AdjustmentDays { get; set; }
    public required string Reason { get; set; }
}