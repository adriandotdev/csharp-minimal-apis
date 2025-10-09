public static class AuthorizationExtensions
{
    public static void ConfigureAuthorization(this IServiceCollection services)
    {
        services.AddAuthorization(options =>
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
    }
}