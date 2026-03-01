using DomnerTech.Backend.Application.Errors;
using Microsoft.AspNetCore.Http;

namespace DomnerTech.Backend.Application.Exceptions;

public class InvalidSortKeyException(string key) : BaseErrorException($"Sort profile '{key}' not found.")
{
    public override int StatusCode => StatusCodes.Status400BadRequest;
    public override string Code => ErrorCodes.Validation;
}