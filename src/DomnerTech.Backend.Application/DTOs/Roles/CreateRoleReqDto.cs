namespace DomnerTech.Backend.Application.DTOs.Roles;

public record CreateRoleReqDto(string Name, string? Desc) : BaseRequest;