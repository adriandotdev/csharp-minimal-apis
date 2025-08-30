using Microsoft.EntityFrameworkCore;

public static class ProductEndpoints
{
    public static void AddProductEndpoints(this IEndpointRouteBuilder app)
    {
        var mapGroup = app.MapGroup("/api/v1/products");

        mapGroup.MapGet("/", GetProducts);

        mapGroup.MapGet("/{id}", GetProductById);

        mapGroup.MapPost("/", CreateProduct);

        mapGroup.MapDelete("/{id}", DeleteProductById);
    }

    private static async Task<IResult> GetProducts(AppDbContext db)
    {
        var products = await db.Products.ToListAsync();

        return TypedResults.Ok(products);
    }

    private static async Task<IResult> GetProductById(int id, AppDbContext db)
    {
        var result = await db.Products.FindAsync(id) is Product product ? Results.Ok(product) : Results.NotFound();

        return TypedResults.Ok(result);
    }

    private static async Task<IResult> CreateProduct(Product product, AppDbContext db)
    {

        db.Products.Add(product);

        await db.SaveChangesAsync();

        return TypedResults.Created($"/products/{product.Id}", product);
    }

    private static async Task<IResult> DeleteProductById(int id, AppDbContext db)
    {
        await db.Products.Where(e => e.Id == id)
        .ExecuteDeleteAsync();

        return TypedResults.Ok(new
        {
            Message = $"Product with ID of {id} is successfully deleted"
        });
    }
}