using Microsoft.AspNetCore.Http;

namespace DomnerTech.Backend.Application.DTOs;

public class BaseResponse
{
    public ResponseStatus Status { get; set; } = new();
}

public class BaseResponse<T>
{
    public ResponseStatus Status { get; set; } = new();
    public T? Data { get; set; }
}

public class ResponseStatus
{
    public string Desc { get; set; } = string.Empty;
    public string ErrorCode { get; set; } = "OK";
    public int StatusCode { get; set; } = StatusCodes.Status200OK;
    public Dictionary<string, string[]> Errors { get; set; } = [];
}