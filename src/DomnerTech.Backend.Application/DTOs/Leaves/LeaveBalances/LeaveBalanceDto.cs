namespace DomnerTech.Backend.Application.DTOs.Leaves.LeaveBalances;

/// <summary>
/// DTO representing leave balance.
/// </summary>
public sealed record LeaveBalanceDto
{
    public required string Id { get; set; }
    public required string EmployeeId { get; set; }
    public required string LeaveTypeId { get; set; }
    public required int Year { get; set; }
    public required decimal TotalAllowance { get; set; }
    public required decimal UsedDays { get; set; }
    public required decimal RemainingDays { get; set; }
    public decimal CarriedForwardDays { get; set; }
    public DateTime? CarryForwardExpiryDate { get; set; }
    public DateTime? LastAccrualDate { get; set; }
}