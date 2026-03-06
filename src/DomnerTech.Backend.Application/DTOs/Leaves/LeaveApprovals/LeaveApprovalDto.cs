using DomnerTech.Backend.Domain.Entities;

namespace DomnerTech.Backend.Application.DTOs.Leaves.LeaveApprovals;

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

public static class LeaveApprovalExtensions
{
    public static LeaveApprovalDto ToDto(this LeaveApprovalEntity entity)
    {
        return new LeaveApprovalDto
        {
            Id = entity.Id.ToString(),
            LeaveRequestId = entity.LeaveRequestId.ToString(),
            Level = entity.Level.ToString(),
            ApproverId = entity.ApproverId.ToString(),
            Status = entity.Status.ToString(),
            Comments = entity.Comments,
            ActionDate = entity.ActionDate,
            SequenceOrder = entity.SequenceOrder,
            IsFinalApproval = entity.IsFinalApproval
        };
    }
}