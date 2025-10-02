using Application.Interfaces;
using Configurations;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using FluentValidation;
using UseCase;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        // Products
        services.AddScoped<GetProductsUseCase>();
        services.AddScoped<CreateProductUseCase>();
        services.AddScoped<GetProductByIdUseCase>();
        services.AddScoped<DeleteProductByIdUseCase>();
        services.AddScoped<UpdateProductUseCase>();
        
        // Categories
        services.AddScoped<GetCategoriesUseCase>();
        services.AddScoped<CreateCategoryUseCase>();
        services.AddScoped<GetCategoryByIdUseCase>();
        services.AddScoped<UpdateCategoryUseCase>();
    
        // Users
        services.AddScoped<CreateUserUseCase>();
        services.AddScoped<LoginUseCase>();
        services.AddScoped<RefreshTokenUseCase>();
        
        // Utils
        services.AddScoped<JwtService>();

        // Validators
        services.AddScoped<IValidator<CreateCategoryRequest>, CreateCategoryValidator>();
        services.AddScoped<IValidator<CreateProductRequest>, CreateProductValidator>();
        services.AddScoped<IValidator<IdRequest>, GetResourceByIdValidator>();
        services.AddScoped<IValidator<CreateUserRequest>, CreateUserValidator>();
        services.AddScoped<IValidator<LoginRequest>, LoginValidator>();
        
        return services;
    }
    
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        
        services.AddDbContext<AppDbContext>((serviceProvider, options) =>
        {
            var dbSettings = serviceProvider.GetRequiredService<IOptions<DatabaseSettings>>().Value;
            options.UseNpgsql(dbSettings.DefaultConnection);
        });

        return services;
    }
}