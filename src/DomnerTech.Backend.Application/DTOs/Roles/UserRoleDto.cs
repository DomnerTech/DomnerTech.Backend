namespace DomnerTech.Backend.Application.DTOs.Roles;

public record UserRoleDto(string UserId, string RoleId, string RoleName, bool HasRole);