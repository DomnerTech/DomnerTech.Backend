namespace DomnerTech.Backend.Application.DTOs.Leaves.LeaveApprovals;

/// <summary>
/// DTO for rejecting a leave request.
/// </summary>
public sealed record RejectLeaveReqDto
{
    public required string LeaveRequestId { get; set; }
    public required string RejectionReason { get; set; }
    public string? Comments { get; set; }
}