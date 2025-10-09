public static class CorsPolicyExtensions
{

    public static void ConfigureCorsPolicy(this IServiceCollection services)
    {

        services.AddCors(option =>
        {
            option.AddDefaultPolicy(builder => builder
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader()
            );
        });
    }
}