using DomnerTech.Backend.Application.Errors;
using Microsoft.AspNetCore.Http;

namespace DomnerTech.Backend.Application.Exceptions;

public sealed class ConflictException(string message) : BaseErrorException(message)
{
    public override int StatusCode => StatusCodes.Status409Conflict;
    public override string Code => ErrorCodes.Conflict;
}