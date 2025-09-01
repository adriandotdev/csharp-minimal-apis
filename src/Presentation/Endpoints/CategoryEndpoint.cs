using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public static class CategoryEndpoints
{

    public static void AddCategoryEndpoints(this IEndpointRouteBuilder app)
    {
        var mapGroup = app.MapGroup("/api/v1/categories");

        mapGroup.MapPost("/", CreateCategory);

        mapGroup.MapGet("/{id}", GetCategoryById);

        mapGroup.MapGet("/", GetCategories);

        mapGroup.MapPut("/{id}", UpdateCategoryById);

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

    private static async Task<IResult> UpdateCategoryById(int id, Category category, AppDbContext db) {

        var categoryToUpdate = await db.Categories.FindAsync(id);

        if (categoryToUpdate is null) return TypedResults.NotFound(new
        {
            Message = $"Category with ID of {id} is not found"
        });

        categoryToUpdate.Name = category.Name ?? categoryToUpdate.Name;
        categoryToUpdate.Description = category.Description ?? categoryToUpdate.Description;

        await db.SaveChangesAsync();

        return TypedResults.Ok(new
        {

            Message = "Category updated successfully",
            category = categoryToUpdate
        });
    }
}