using DomnerTech.Backend.Application.Constants;
using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace DomnerTech.Backend.Api.ApiDocs;

public class HeadersOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        operation.Parameters ??= [];

        operation.Parameters.Add(new OpenApiParameter
        {
            Name = HeaderConstants.CorrelationId,
            In = ParameterLocation.Header,
            Required = true,
            Description = "Correlation ID for request tracking",
            Schema = new OpenApiSchema
            {
                Type = JsonSchemaType.String
            },
            Example = "550e8400-e29b-41d4-a716-446655440000"
        });
        operation.Parameters.Add(new OpenApiParameter
        {
            Name = HeaderConstants.Lang,
            In = ParameterLocation.Header,
            Required = true,
            Description = "User language",
            Schema = new OpenApiSchema
            {
                Type = JsonSchemaType.String
            },
            Example = "km"
        });
        operation.Parameters.Add(new OpenApiParameter
        {
            Name = HeaderConstants.Platform,
            In = ParameterLocation.Header,
            Required = true,
            Description = "User platform",
            Schema = new OpenApiSchema
            {
                Type = JsonSchemaType.String
            },
            Example = "chrome"
        });
    }
}