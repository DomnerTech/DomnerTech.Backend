using DomnerTech.Backend.Application;
using Microsoft.OpenApi;

namespace DomnerTech.Backend.Api.ApiDocs;

public static class SwaggerExtension
{
    public static IServiceCollection AddSwaggerDoc(this IServiceCollection services, AppSettings appSettings, string xmlCommentFile)
    {
        if (!appSettings.Swagger.Enable) return services;
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "DomnerTech Service API Document", Version = "v1" });
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Bearer",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = "Bearer"
            });

            c.AddSecurityRequirement(document => new OpenApiSecurityRequirement
            {
                [new OpenApiSecuritySchemeReference("Bearer", document)] = []
            });
            c.OperationFilter<HeadersOperationFilter>();
            c.IncludeXmlComments(Path.Join(AppContext.BaseDirectory, xmlCommentFile));
            c.DocumentFilter<BasePathDocumentFilter>();
            c.OperationFilter<SnakeCaseQueryParameterFilter>();
        });
        return services;
    }

    public static IApplicationBuilder UseSwaggerDoc(this IApplicationBuilder app, AppSettings appSettings)
    {
        if (!appSettings.Swagger.Enable) return app;
        app.UseSwagger();
        app.UseSwaggerUI();
        return app;
    }
}