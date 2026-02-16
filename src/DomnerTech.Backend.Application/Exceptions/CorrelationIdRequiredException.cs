using DomnerTech.Backend.Application.Errors;
using Microsoft.AspNetCore.Http;

namespace DomnerTech.Backend.Application.Exceptions;

public sealed class CorrelationIdRequiredException() : BaseErrorException("X-Correlation-Id header is required")
{
    public override int StatusCode => StatusCodes.Status400BadRequest;
    public override string Code => ErrorCodes.HeaderMissing;
}