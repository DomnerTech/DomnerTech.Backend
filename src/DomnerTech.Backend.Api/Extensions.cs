using DomnerTech.Backend.Application.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace DomnerTech.Backend.Api;

public static class Extensions
{
    /// <summary>
    /// Creates a new JsonResult containing the specified response object and sets the HTTP status code based on the
    /// response status.
    /// </summary>
    /// <remarks>This method is intended for use in ASP.NET Core controllers to return standardized JSON
    /// responses with appropriate status codes. The status code is taken from the Status property of the response
    /// object.</remarks>
    /// <typeparam name="T">The type of the data contained in the response.</typeparam>
    /// <param name="obj">The response object to serialize to JSON. Must not be null.</param>
    /// <returns>A JsonResult that serializes the specified response object and uses its status code.</returns>
    public static JsonResult ReturnJson<T>(this BaseResponse<T> obj)
    {
        return new JsonResult(obj)
        {
            StatusCode = obj.Status.StatusCode
        };
    }

}