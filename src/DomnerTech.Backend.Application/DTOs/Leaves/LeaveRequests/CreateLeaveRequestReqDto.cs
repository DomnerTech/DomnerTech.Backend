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