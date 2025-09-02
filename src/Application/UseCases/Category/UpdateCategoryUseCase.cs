public class UpdateCategoryUseCase
{
    private readonly ICategoryRepository _categoryRepository;

    public UpdateCategoryUseCase(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<Response<Category>> Handle(int id, CreateCategoryRequest request)
    {
        var category = await _categoryRepository.UpdateCategory(id, request);

        if (category is null) return new Response<Category>(Status.NotFound, default, $"Category with ID of {id} is not found");

        return new Response<Category>(Status.OK, category);
    }
}