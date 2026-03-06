using DomnerTech.Backend.Domain.Entities;
using DomnerTech.Backend.Domain.Enums;

namespace DomnerTech.Backend.Application.DTOs.Leaves.LeaveRequests;

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

public static class LeaveRequestExtensions
{
    public static LeaveRequestDto ToDto(this LeaveRequestEntity entity)
    {
        return new LeaveRequestDto
        {
            Id = entity.Id.ToString(),
            EmployeeId = entity.EmployeeId.ToString(),
            LeaveTypeId = entity.LeaveTypeId.ToString(),
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
            CreatedAt = entity.CreatedAt
        };
    }
}