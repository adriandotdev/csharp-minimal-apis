public static class SwaggerExtensions
{
    public static void ConfigureSwagger(this IServiceCollection services)
    {
        services.AddOpenApiDocument(config =>
        {
            config.DocumentName = "inventory-management-system";
            config.Title = "API V1";
            config.Version = "v1";

           config.AddSecurity("JWT", Enumerable.Empty<string>(), new NSwag.OpenApiSecurityScheme
            {
                Type = NSwag.OpenApiSecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT",
                In = NSwag.OpenApiSecurityApiKeyLocation.Header,
                Name = "Authorization",
                Description = "Enter JWT token like: **Bearer your_token_here**"
            });
        });
    }
}