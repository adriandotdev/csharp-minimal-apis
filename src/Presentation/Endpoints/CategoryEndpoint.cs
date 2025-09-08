using Microsoft.AspNetCore.Mvc;

public static class CategoryEndpoints
{
    // @TODO: Add Authorization to all endpoitns
    public static void AddCategoryEndpoints(this IEndpointRouteBuilder app)
    {
        var mapGroup = app.MapGroup("/api/v1/categories");

        mapGroup.MapPost("/", CreateCategory).RequireAuthorization("admin_access");

        mapGroup.MapGet("/{id}", GetCategoryById).RequireAuthorization("general_access");

        mapGroup.MapGet("/", GetCategories).RequireAuthorization("general_access");

        mapGroup.MapPut("/{id}", UpdateCategoryById).RequireAuthorization("admin_access");
    }

    private static async Task<IResult> CreateCategory([FromBody] CreateCategoryRequest request, [FromServices] CreateCategoryUseCase useCase)
    {
        var result = await useCase.Handle(request);

        return Response<IResult>.MapResponse(result.Status, result.Data, result.Message);
    }

    private static async Task<IResult> GetCategories([FromServices] GetCategoriesUseCase useCase)
    {
        var result = await useCase.Handle();

        return Response<IResult>.MapResponse(result.Status, result.Data, result.Message);
    }

    private static async Task<IResult> GetCategoryById(int id, [FromServices] GetCategoryByIdUseCase useCase)
    {
        var result = await useCase.Handle(id);

        return Response<IResult>.MapResponse(result.Status, result.Data, result.Message);
    }

    private static async Task<IResult> UpdateCategoryById(int id, [FromBody] CreateCategoryRequest request, [FromServices] UpdateCategoryUseCase useCase)
    {
        var result = await useCase.Handle(id, request);

        return Response<IResult>.MapResponse(result.Status, result.Data, result.Message);
    }
}