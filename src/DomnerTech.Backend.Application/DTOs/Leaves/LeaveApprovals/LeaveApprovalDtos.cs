namespace DomnerTech.Backend.Application.DTOs.Leaves.LeaveApprovals;

/// <summary>
/// DTO for approving a leave request.
/// </summary>
public sealed record ApproveLeaveReqDto
{
    public required string LeaveRequestId { get; set; }
    public string? Comments { get; set; }
}

/// <summary>
/// DTO for rejecting a leave request.
/// </summary>
public sealed record RejectLeaveReqDto
{
    public required string LeaveRequestId { get; set; }
    public required string RejectionReason { get; set; }
    public string? Comments { get; set; }
}

/// <summary>
/// DTO representing a leave approval.
/// </summary>
public sealed record LeaveApprovalDto
{
    public required string Id { get; set; }
    public required string LeaveRequestId { get; set; }
    public required string Level { get; set; }
    public required string ApproverId { get; set; }
    public required string Status { get; set; }
    public string? Comments { get; set; }
    public DateTime? ActionDate { get; set; }
    public int SequenceOrder { get; set; }
    public bool IsFinalApproval { get; set; }
}
