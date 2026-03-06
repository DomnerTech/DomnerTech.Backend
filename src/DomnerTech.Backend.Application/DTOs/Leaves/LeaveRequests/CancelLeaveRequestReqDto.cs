namespace DomnerTech.Backend.Application.DTOs.Leaves.LeaveRequests;

/// <summary>
/// DTO for cancelling a leave request.
/// </summary>
public sealed record CancelLeaveRequestReqDto
{
    public required string Id { get; set; }
    public required string CancellationReason { get; set; }
}