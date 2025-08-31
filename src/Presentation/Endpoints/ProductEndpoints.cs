using Microsoft.AspNetCore.Mvc;
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
    
    private static async Task<IResult> CreateProduct([FromServices] CreateProductUseCase useCase, [FromBody] CreateProductRequest request)
    {
        var product = await useCase.Handle(request);

        return TypedResults.Created($"/products/{product.Id}", product);
    }

    private static async Task<IResult> GetProducts([FromServices] GetProductsUseCase useCase)
    {
        var products = await useCase.Handle();

        return TypedResults.Ok(products);
    }

    private static async Task<IResult> GetProductById(int id, AppDbContext db)
    {
        var result = await db.Products.FindAsync(id) is Product product ? Results.Ok(product) : Results.NotFound();

        return TypedResults.Ok(result);
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