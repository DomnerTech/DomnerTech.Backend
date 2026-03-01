using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Text.Json;

namespace DomnerTech.Backend.Api.ApiDocs;

public sealed class SnakeCaseQueryParameterFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        if (operation.Parameters == null)
            return;

        foreach (var parameter in operation.Parameters.ToList())
        {
            // Fix CS8604: Check for null before converting
            var originalName = parameter.Name;
            if (originalName == null) continue;
            // Fix CS0200: Cannot assign to read-only property, so create a new parameter with the snake_case name
            // However, OpenApiParameter (from Swashbuckle) is usually mutable, but if it's not, you cannot change the name.
            // Instead, you can replace the parameter with a new one if the type allows.
            // If the type is not mutable, you must remove and add a new parameter.
            // Here is a safe approach:
            var snakeCaseName = JsonNamingPolicy.SnakeCaseLower.ConvertName(originalName);

            // Only update if the name actually changes
            if (snakeCaseName == originalName) continue;
            // Remove the old parameter and add a new one with the snake_case name
            // This assumes OpenApiOperation.Parameters is a modifiable list and parameter is of type OpenApiParameter
            // If not, you may need to cast or create a new list
            var index = operation.Parameters.IndexOf(parameter);
            if (index < 0) continue;
            var newParameter = new OpenApiParameter
            {
                Name = snakeCaseName,
                In = parameter.In,
                Required = parameter.Required,
                Deprecated = parameter.Deprecated,
                AllowEmptyValue = parameter.AllowEmptyValue,
                Style = parameter.Style,
                Explode = parameter.Explode,
                AllowReserved = parameter.AllowReserved,
                Schema = parameter.Schema,
                Examples = parameter.Examples,
                Example = parameter.Example,
                Content = parameter.Content,
                Description = parameter.Description,
                Extensions = parameter.Extensions
            };
            operation.Parameters[index] = newParameter;
        }
    }
}