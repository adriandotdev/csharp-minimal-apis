using System.Text;
using System.Text.Json.Serialization;
using Configurations;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

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

builder.Services.AddAuthentication().AddJwtBearer(options =>
    {
        var config = builder.Configuration.GetSection("Authentication:Schemes:Bearer");
        
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = config["ValidIssuer"],
            ValidAudience = config["ValidAudience"],
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["IssuerSigningKey"]!))
        };
    });


builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("admin_access",
        policy =>
            policy
                .RequireRole("Admin")
                .RequireClaim("scope", "admin_scope")
                .RequireClaim("token_type", "access")
    );

    options.AddPolicy("staff_access",
        policy =>
            policy
                .RequireRole("Staff")
                .RequireClaim("scope", "staff_scope")
                .RequireClaim("token_type", "access")
    );

    options.AddPolicy("general_access",
        policy =>
            policy
                .RequireRole("Admin", "Staff")
                .RequireClaim("scope", ["staff_scope", "admin_scope"])
                .RequireClaim("token_type", "access")
    );

});

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
