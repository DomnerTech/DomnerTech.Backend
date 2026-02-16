namespace DomnerTech.Backend.Application.Exceptions;

public abstract class BaseErrorException(string message) : Exception(message)
{
    public abstract int StatusCode { get; }
    public abstract string Code { get; }
}
