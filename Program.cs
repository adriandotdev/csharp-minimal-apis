using Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<DatabaseSettings>(builder.Configuration.GetSection("ConnectionStrings"));


builder.Services.AddDbContext<AppDbContext>((serviceProvider, options) =>
{
    var dbSettings = serviceProvider.GetRequiredService<IOptions<DatabaseSettings>>().Value;

    options.UseNpgsql(dbSettings.DefaultConnection);
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApiDocument(config =>
{
    config.DocumentName = "Product Management System";
    config.Title = "API V1";
    config.Version = "v1";
});

var app = builder.Build();

app.Run();
