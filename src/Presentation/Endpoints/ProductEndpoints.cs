using FluentValidation;
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
    
    private static async Task<IResult> CreateProduct([FromServices] CreateProductUseCase useCase, [FromBody] CreateProductRequest request, IValidator<CreateProductRequest> validator)
    {

        var validationResult = await validator.ValidateAsync(request);

        if (!validationResult.IsValid)
        {
            return TypedResults.BadRequest(Response<IResult>.MapErrors(validationResult));
        }

        var result = await useCase.Handle(request);

        return Response<IResult>.MapResponse(result.Status, result.Data, $"/api/v1/products/{result?.Data?.Id}");
    }

    private static async Task<IResult> GetProducts([FromServices] GetProductsUseCase useCase)
    {
        var result = await useCase.Handle();

        return Response<IResult>.MapResponse(result.Status, result.Data, result.Message);
    }

    private static async Task<IResult> GetProductById(string id, [FromServices] GetProductByIdUseCase useCase, IValidator<IdRequest> validator)
    {
        var request = new IdRequest { Id = id };
        var validationResult = await validator.ValidateAsync(request);

        if (!validationResult.IsValid)
        {
            return TypedResults.BadRequest(Response<IResult>.MapErrors(validationResult));
        }
        
        var result = await useCase.Handle(int.Parse(id));

        return Response<IResult>.MapResponse(result.Status, data: result.Data, result.Message);
    }

    private static async Task<IResult> DeleteProductById(string id, [FromServices] DeleteProductByIdUseCase useCase, IValidator<IdRequest> validator)
    {
        var request = new IdRequest { Id = id };
        var validationResult = await validator.ValidateAsync(request);

        if (!validationResult.IsValid)
        {
            return TypedResults.BadRequest(Response<IResult>.MapErrors(validationResult));
        }

        var result = await useCase.Handle(int.Parse(id));

        return Response<IResult>.MapResponse(result.Status, result.Data, result.Message);
    }
}