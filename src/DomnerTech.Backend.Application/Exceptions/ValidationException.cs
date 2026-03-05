using DomnerTech.Backend.Application.Errors;
using Microsoft.AspNetCore.Http;

namespace DomnerTech.Backend.Application.Exceptions;

/// <summary>
/// Exception for validation failures.
/// </summary>
public sealed class ValidationException(string message) : BaseErrorException(message)
{
    public override int StatusCode => StatusCodes.Status400BadRequest;
    public override string Code => ErrorCodes.Validation;
}
