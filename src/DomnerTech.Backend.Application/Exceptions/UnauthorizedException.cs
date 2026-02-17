using DomnerTech.Backend.Application.Errors;
using Microsoft.AspNetCore.Http;

namespace DomnerTech.Backend.Application.Exceptions;

public sealed class UnauthorizedException() : BaseErrorException("User not authorized")
{
    public override int StatusCode => StatusCodes.Status401Unauthorized;
    public override string Code => ErrorCodes.Unauthorized;
}