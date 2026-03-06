using DomnerTech.Backend.Domain.Entities;

namespace DomnerTech.Backend.Application.DTOs.Leaves.LeaveRequests;

/// <summary>
/// DTO for detailed leave request with related data.
/// </summary>
public sealed record LeaveRequestDetailDto : LeaveRequestDto
{
    public string? LeaveTypeName { get; set; }
    public string? EmployeeName { get; set; }
    public string? ApproverName { get; set; }
}

public static class LeaveRequestDetailExtensions
{
    public static LeaveRequestDetailDto ToDto(this LeaveRequestEntity entity,
        string? leaveTypeName = null,
        string? employeeName = null,
        string? approverName = null)
    {
        return new LeaveRequestDetailDto
        {
            Id = entity.Id.ToString(),
            EmployeeId = entity.EmployeeId.ToString(),
            EmployeeName = employeeName,
            LeaveTypeId = entity.LeaveTypeId.ToString(),
            LeaveTypeName = leaveTypeName,
            StartDate = entity.Period.StartDate,
            EndDate = entity.Period.EndDate,
            RequestType = entity.RequestType,
            TotalDays = entity.TotalDays,
            Reason = entity.Reason,
            Notes = entity.Notes,
            DocumentUrls = entity.DocumentUrls,
            Status = entity.Status,
            SubmittedAt = entity.SubmittedAt,
            CurrentApprovalLevel = entity.CurrentApprovalLevel,
            ApprovedBy = entity.ApprovedBy?.ToString(),
            ApprovedAt = entity.ApprovedAt,
            RejectionReason = entity.RejectionReason,
            CancellationReason = entity.CancellationReason,
            CancelledAt = entity.CancelledAt,
            CreatedAt = entity.CreatedAt,
            ApproverName = approverName
        };
    }
}