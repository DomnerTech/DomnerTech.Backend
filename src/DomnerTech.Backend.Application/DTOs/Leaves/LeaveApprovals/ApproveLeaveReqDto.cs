namespace DomnerTech.Backend.Application.DTOs.Leaves.LeaveApprovals;

/// <summary>
/// DTO for approving a leave request.
/// </summary>
public sealed record ApproveLeaveReqDto
{
    public required string LeaveRequestId { get; set; }
    public string? Comments { get; set; }
}