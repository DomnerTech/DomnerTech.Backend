using DomnerTech.Backend.Domain.Enums;

namespace DomnerTech.Backend.Application.DTOs.Leaves.LeaveRequests;

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