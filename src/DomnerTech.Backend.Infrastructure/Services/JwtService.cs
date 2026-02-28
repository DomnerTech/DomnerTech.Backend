using DomnerTech.Backend.Application;
using DomnerTech.Backend.Application.Constants;
using DomnerTech.Backend.Application.Exceptions;
using DomnerTech.Backend.Application.Services;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using DomnerTech.Backend.Application.DTOs.Users;

namespace DomnerTech.Backend.Infrastructure.Services;

public sealed class JwtService(
    ILogger<JwtService> logger,
    AppSettings appSettings) : IJwtService
{
    public async Task<string> CreateTokenAsync(UserDto user, CancellationToken cancellationToken = default)
    {
        try
        {
            var key = Encoding.UTF8.GetBytes(appSettings.BearerAuth.SecretKey);
            var expiryInMinutes = appSettings.BearerAuth.ExpiryInMinutes;

            List<Claim> claims =
            [
                new(ClaimConstant.UserId, user.Id),
                new(ClaimConstant.CompanyId, user.CompanyId),
                new(ClaimTypes.NameIdentifier, user.Username)
            ];

            claims.AddRange(user.Roles.Select(r => new Claim(ClaimTypes.Role, r)));

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(expiryInMinutes),
                SigningCredentials =
                    new SigningCredentials(
                        new SymmetricSecurityKey(key),
                        SecurityAlgorithms.HmacSha256Signature),
                Issuer = appSettings.BearerAuth.Issuer,
                Audience = appSettings.BearerAuth.Audience

            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return await Task.FromResult(tokenHandler.WriteToken(token));
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error while generate jwt.");
            throw new CreateJwtException();
        }
    }
}