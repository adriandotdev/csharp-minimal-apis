using Application.Interfaces;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<GetProductsUseCase>();
        services.AddScoped<CreateProductUseCase>();
        services.AddScoped<GetProductByIdUseCase>();
        services.AddScoped<DeleteProductByIdUseCase>();
        
        return services;
    }
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<IProductRepository, ProductRepository>();

        return services;
    }
}