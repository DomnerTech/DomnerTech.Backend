using DomnerTech.Backend.Application;
using DomnerTech.Backend.Application.Constants;
using DomnerTech.Backend.Application.Exceptions;
using DomnerTech.Backend.Application.Services;
using DomnerTech.Backend.Domain.Entities;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using DomnerTech.Backend.Application.IRepo;

namespace DomnerTech.Backend.Infrastructure.Services;

public sealed class JwtService(
    ILogger<JwtService> logger,
    AppSettings appSettings,
    IPolicyRepo policyRepo) : IJwtService
{
    public async Task<string> CreateTokenAsync(UserEntity user, CancellationToken cancellationToken = default)
    {
        try
        {
            var key = Encoding.UTF8.GetBytes(appSettings.BearerAuth.SecretKey);
            var expiryInMinutes = appSettings.BearerAuth.ExpiryInMinutes;

            List<Claim> claims =
            [
                new(ClaimConstant.UserId, user.Id.ToString()),
                new(ClaimConstant.CompanyId, user.CompanyId.ToString()),
                new(ClaimTypes.NameIdentifier, user.Username)
            ];

            var policies = await policyRepo.GetByNamesAsync(user.Policies, cancellationToken);

            foreach (var policy in policies)
            {
                claims.AddRange(policy.RequiredRoles
                    .Select(p => new Claim(ClaimConstant.Roles, p)));
            }

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