using FluentValidation;
using Microsoft.AspNetCore.Mvc;

public static class CategoryEndpoints
{
    public static void AddCategoryEndpoints(this IEndpointRouteBuilder app)
    {
        var mapGroup = app.MapGroup("/api/v1/categories");

        mapGroup.MapPost("/", CreateCategory).RequireAuthorization("admin_access");

        mapGroup.MapGet("/{id}", GetCategoryById).RequireAuthorization("general_access");

        mapGroup.MapGet("/", GetCategories).RequireAuthorization("general_access");

        mapGroup.MapPut("/{id}", UpdateCategoryById).RequireAuthorization("admin_access");
    }

    private static async Task<IResult> CreateCategory([FromBody] CreateCategoryRequest request, [FromServices] CreateCategoryUseCase useCase, IValidator<CreateCategoryRequest> validator)
    {
        var validationResult = await validator.ValidateAsync(request);

        if (!validationResult.IsValid)
        {
            return TypedResults.BadRequest(Response<IResult>.MapErrors(validationResult));
        }

        var result = await useCase.Handle(request);

        return Response<IResult>.MapResponse(result.Status, result.Data, result.Message);
    }

    private static async Task<IResult> GetCategories([FromServices] GetCategoriesUseCase useCase)
    {
        var result = await useCase.Handle();

        return Response<IResult>.MapResponse(result.Status, result.Data, result.Message);
    }

    private static async Task<IResult> GetCategoryById(string id, [FromServices] GetCategoryByIdUseCase useCase, IValidator<IdRequest> validator)
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

    private static async Task<IResult> UpdateCategoryById(
        string id,
        [FromBody] CreateCategoryRequest request,
        [FromServices] UpdateCategoryUseCase useCase,
        IValidator<IdRequest> parameterValidator,
        IValidator<CreateCategoryRequest> requestBodyValidator)
    {
        var parameterRequest = new IdRequest { Id = id };
        var parameterValidationResult = await parameterValidator.ValidateAsync(parameterRequest);
        var requestBodyValidationResult = await requestBodyValidator.ValidateAsync(request);

        if (!parameterValidationResult.IsValid)
        {
            return TypedResults.BadRequest(Response<IResult>.MapErrors(parameterValidationResult));
        }

        if (!requestBodyValidationResult.IsValid)
        {
            return TypedResults.BadRequest(Response<IResult>.MapErrors(requestBodyValidationResult));
        }

        var result = await useCase.Handle(int.Parse(id), request);

        return Response<IResult>.MapResponse(result.Status, result.Data, result.Message);
    }
}