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
        var result = await useCase.Handle(request);

        return Response<IResult>.MapResponse(result.Status, result.Data, $"/api/v1/products/{result?.Data?.Id}");
    }

    private static async Task<IResult> GetProducts([FromServices] GetProductsUseCase useCase)
    {
        var result = await useCase.Handle();

        return Response<IResult>.MapResponse(result.Status, result.Data, result.Message);
    }

    private static async Task<IResult> GetProductById(int id, [FromServices] GetProductByIdUseCase useCase)
    {
        var result = await useCase.Handle(id);

        return Response<IResult>.MapResponse(result.Status, data: result.Data, result.Message);
    }

    private static async Task<IResult> DeleteProductById(int id, [FromServices] DeleteProductByIdUseCase useCase)
    {
        var result = await useCase.Handle(id);

        return Response<IResult>.MapResponse(result.Status, result.Data, result.Message);
    }
}