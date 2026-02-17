using DomnerTech.Backend.Application.DTOs.Users;

namespace DomnerTech.Backend.Application.DTOs.Auth;

public class LoginResDto
{
    public required string Token { get; set; }
    public required UserDto User { get; set; }
}