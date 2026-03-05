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
