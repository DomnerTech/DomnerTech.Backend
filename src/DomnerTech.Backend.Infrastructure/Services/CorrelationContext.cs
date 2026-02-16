using DomnerTech.Backend.Application.Constants;
using Microsoft.AspNetCore.Http;
using Mobile.CleanArchProjectTemplate.Application.Services;

namespace DomnerTech.Backend.Infrastructure.Services;

public sealed class CorrelationContext(IHttpContextAccessor accessor) : ICorrelationContext
{
    public string CorrelationId =>
        accessor.HttpContext?.Items[HeaderConstants.CorrelationContextKey]?.ToString()
        ?? "N/A";
}