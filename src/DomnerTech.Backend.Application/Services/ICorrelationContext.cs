namespace Mobile.CleanArchProjectTemplate.Application.Services;

public interface ICorrelationContext
{
    string CorrelationId { get; }
}