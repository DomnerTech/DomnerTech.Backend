using DomnerTech.Backend.Application;
using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace DomnerTech.Backend.Api.ApiDocs;

public class BasePathDocumentFilter(AppSettings appSettings) : IDocumentFilter
{
    /// <inheritdoc/>
    public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
    {
        swaggerDoc.Servers = [.. appSettings.Swagger.Urls.Select(x => new OpenApiServer { Url = x })];
    }
}