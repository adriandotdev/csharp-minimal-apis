using Microsoft.AspNetCore.Mvc;

public static class ProductEndpoints
{
    public static void AddProductEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/v1/products");

        group.MapGet("/", GetProducts).RequireAuthorization("general_access");
   
        group.MapGet("/{id}", GetProductById).RequireAuthorization("general_access");

        group.MapPost("/", CreateProduct).RequireAuthorization("admin_access");

        group.MapDelete("/{id}", DeleteProductById).RequireAuthorization("admin_access");
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