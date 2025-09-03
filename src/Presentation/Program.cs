using System.Text.Json.Serialization;
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

builder.Services.AddInfrastructure();
builder.Services.AddApplicationServices();

builder.Services.AddCors();
builder.Services.AddAuthentication().AddJwtBearer("Bearer");
builder.Services.AddAuthorization();

builder.Services.AddAuthorizationBuilder()
    .AddPolicy("admin_sample", policy => policy.RequireRole("admin").RequireClaim("scope", "admin_scope"))
    .AddPolicy("user_sample", policy => policy.RequireRole("user").RequireClaim("scope", "user_scope"));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApiDocument(config =>
{
    config.DocumentName = "Product Management System";
    config.Title = "API V1";
    config.Version = "v1";
});
builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseOpenApi();
    app.UseSwaggerUi(config =>
    {
        config.DocumentTitle = "PMS";
        config.Path = "/swagger";
        config.DocumentPath = "/swagger/pms/swagger.json";
        config.DocExpansion = "list";
    });
}
app.UseCors();
app.UseAuthentication();
app.UseAuthorization();

app.AddProductEndpoints();
app.AddCategoryEndpoints();
app.AddAuthenticationEndpoints();

app.Run();
