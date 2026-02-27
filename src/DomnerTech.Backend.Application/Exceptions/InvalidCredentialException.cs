using DomnerTech.Backend.Application.Errors;
using Microsoft.AspNetCore.Http;

namespace DomnerTech.Backend.Application.Exceptions;

public class InvalidCredentialException() : BaseErrorException("Invalid credential, please try again!")
{
    public override int StatusCode => StatusCodes.Status401Unauthorized;
    public override string Code => ErrorCodes.Unauthorized;
}