using Microsoft.EntityFrameworkCore;

public static class CategoryEndpoints
{

    public static void AddCategoryEndpoints(this IEndpointRouteBuilder app)
    {

        var mapGroup = app.MapGroup("/api/v1/categories");

        mapGroup.MapPost("/", async (Category category, AppDbContext db) =>
        {
            db.Categories.Add(category);

            await db.SaveChangesAsync();

            return Results.Created($"/api/v1/categories/{category.Id}", category);
        });

        mapGroup.MapGet("/{id}", async (int id, AppDbContext db) =>
        {
            var category = await db.Categories.FindAsync(id);

            return Results.Ok(category);
        });

        mapGroup.MapGet("/", async (AppDbContext db) =>
        {
            return await db.Categories.ToListAsync();
        });
    }
}