using DomnerTech.Backend.Application.Constants;
using DomnerTech.Backend.Application.Services;
using Microsoft.AspNetCore.Http;

namespace DomnerTech.Backend.Infrastructure.Services;

public sealed class CorrelationContext(IHttpContextAccessor accessor) : ICorrelationContext
{
    public string CorrelationId =>
        accessor.HttpContext?.Items[HeaderConstants.CorrelationContextKey]?.ToString()
        ?? "N/A";
}