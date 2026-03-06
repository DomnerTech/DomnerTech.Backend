namespace DomnerTech.Backend.Application.DTOs.Leaves.LeaveBalances;

/// <summary>
/// DTO for initializing leave balance for an employee.
/// </summary>
public sealed record InitializeLeaveBalanceReqDto
{
    public required string EmployeeId { get; set; }
    public required string LeaveTypeId { get; set; }
    public required int Year { get; set; }
    public required decimal TotalAllowance { get; set; }
    public decimal CarriedForwardDays { get; set; }
}