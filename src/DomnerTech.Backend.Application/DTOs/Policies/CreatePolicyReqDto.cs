namespace DomnerTech.Backend.Application.DTOs.Policies;

public sealed record CreatePolicyReqDto(string Name, HashSet<string> RoleNames, string? Desc) : BaseRequest;