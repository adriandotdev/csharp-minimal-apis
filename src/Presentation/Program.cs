using System.Text;
using System.Text.Json.Serialization;
using Configurations;
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
    config.DocumentName = "inventory-management-system";
    config.Title = "API V1";
    config.Version = "v1";

    // 👇 Add this part
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
builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
});
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
    });
    
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseOpenApi();
    app.UseSwaggerUi(config =>
    {
        config.DocumentTitle = "Inventory Management System";
        config.Path = "/swagger";
        config.DocumentPath = "/swagger/inventory-management-system/swagger.json";
        config.DocExpansion = "list";
        config.PersistAuthorization = true;
    });
}

app.UseRouting();
app.UseCors();
app.UseAuthentication();
app.UseAuthorization();

app.AddAuthenticationEndpoints();
app.AddProductEndpoints();
app.AddCategoryEndpoints();


app.Run();
