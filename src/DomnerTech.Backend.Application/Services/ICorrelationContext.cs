namespace DomnerTech.Backend.Application.Services;

public interface ICorrelationContext
{
    string CorrelationId { get; }
}