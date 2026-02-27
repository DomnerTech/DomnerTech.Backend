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
            }
        });
    }
}