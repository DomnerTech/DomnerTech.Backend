namespace DomnerTech.Backend.Application.Errors;

public class ErrorCodes
{
    protected ErrorCodes(){ }
    public const string Validation = "validation_error";
    public const string NotFound = "not_found";
    public const string Conflict = "conflict";
    public const string Unauthorized = "unauthorized";
    public const string Forbidden = "forbidden";
    public const string Internal = "internal_server_error";
    public const string HeaderMissing = "header_missing";
}