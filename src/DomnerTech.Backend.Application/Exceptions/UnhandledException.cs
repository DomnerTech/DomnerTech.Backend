using DomnerTech.Backend.Application.Errors;
using Microsoft.AspNetCore.Http;

namespace DomnerTech.Backend.Application.Exceptions;

public sealed class UnhandledException(string message) : BaseErrorException(message)
{
    public override int StatusCode => StatusCodes.Status500InternalServerError;
    public override string Code => ErrorCodes.SystemError;
}