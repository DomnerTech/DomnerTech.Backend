using DomnerTech.Backend.Domain.Enums;

namespace DomnerTech.Backend.Application.DTOs.Leaves.LeaveRequests;

/// <summary>
/// DTO for creating a leave request.
/// </summary>
public sealed record CreateLeaveRequestReqDto
{
    public required string LeaveTypeId { get; set; }
    public required DateTime StartDate { get; set; }
    public required DateTime EndDate { get; set; }
    public LeaveRequestType RequestType { get; set; } = LeaveRequestType.FullDay;
    public string? Reason { get; set; }
    public string? Notes { get; set; }
    public List<string>? DocumentUrls { get; set; }
}

/// <summary>
/// DTO for updating a leave request.
/// </summary>
public sealed record UpdateLeaveRequestReqDto
{
    public required string Id { get; set; }
    public required DateTime StartDate { get; set; }
    public required DateTime EndDate { get; set; }
    public LeaveRequestType RequestType { get; set; }
    public string? Reason { get; set; }
    public string? Notes { get; set; }
    public List<string>? DocumentUrls { get; set; }
}

/// <summary>
/// DTO for cancelling a leave request.
/// </summary>
public sealed record CancelLeaveRequestReqDto
{
    public required string Id { get; set; }
    public required string CancellationReason { get; set; }
}

/// <summary>
/// DTO representing a leave request.
/// </summary>
public record LeaveRequestDto
{
    public required string Id { get; set; }
    public required string EmployeeId { get; set; }
    public required string LeaveTypeId { get; set; }
    public required DateTime StartDate { get; set; }
    public required DateTime EndDate { get; set; }
    public LeaveRequestType RequestType { get; set; }
    public required decimal TotalDays { get; set; }
    public string? Reason { get; set; }
    public string? Notes { get; set; }
    public List<string>? DocumentUrls { get; set; }
    public LeaveRequestStatus Status { get; set; }
    public DateTime SubmittedAt { get; set; }
    public ApprovalLevel? CurrentApprovalLevel { get; set; }
    public string? ApprovedBy { get; set; }
    public DateTime? ApprovedAt { get; set; }
    public string? RejectionReason { get; set; }
    public string? CancellationReason { get; set; }
    public DateTime? CancelledAt { get; set; }
    public DateTime CreatedAt { get; set; }
}

/// <summary>
/// DTO for detailed leave request with related data.
/// </summary>
public sealed record LeaveRequestDetailDto : LeaveRequestDto
{
    public string? LeaveTypeName { get; set; }
    public string? EmployeeName { get; set; }
    public string? ApproverName { get; set; }
}
