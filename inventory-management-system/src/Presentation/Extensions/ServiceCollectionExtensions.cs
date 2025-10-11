using Application.Interfaces;
using Configurations;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using FluentValidation;
using UseCase;
using Scrutor;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {   
        // Map application use-cases
        services.Scan(scan => scan
            .FromAssemblies(typeof(Application.UseCases.UseCaseAssembly).Assembly)
            .AddClasses(filter => filter.Where(className => className.Name.EndsWith("UseCase")))
            .UsingRegistrationStrategy(RegistrationStrategy.Skip)
            .AsSelf()
            .WithScopedLifetime());

        // Map concrete validator classes
        services.Scan(scan => scan
            .FromAssemblies(typeof(Application.Validators.ValidatorsAssembly).Assembly)
            .AddClasses(filter => filter.Where(className => className.Name.EndsWith("Validator")))
            .UsingRegistrationStrategy(RegistrationStrategy.Skip)
            .AsImplementedInterfaces()
            .WithScopedLifetime());
        
        services.AddScoped<JwtService>();
        
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