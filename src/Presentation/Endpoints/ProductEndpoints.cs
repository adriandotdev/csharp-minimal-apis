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
        var response = await useCase.Handle(request);

        return Response<Product>.MapResponse<object>(response.Status, response.Data, $"/api/v1/products/{response?.Data?.Id}");
    }

    private static async Task<IResult> GetProducts([FromServices] GetProductsUseCase useCase)
    {
        var response = await useCase.Handle();

        return Response<ICollection<Product>>.MapResponse(response.Status, response.Data);
    }

    private static async Task<IResult> GetProductById(int id, [FromServices] GetProductByIdUseCase useCase)
    {
        var result = await useCase.Handle(id);

        return Response<IResult>.MapResponse(result.Status, data: result.Data);
    }

    private static async Task<IResult> DeleteProductById(int id, [FromServices] DeleteProductByIdUseCase useCase)
    {
        var result = await useCase.Handle(id);

        return Response<IResult>.MapResponse(result.Status, result.Data, result.Message);
    }
}