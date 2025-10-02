public class CreateCategoryUseCase
{

    private readonly ICategoryRepository _categoryRepository;

    public CreateCategoryUseCase(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<Response<Category>> Handle(CreateCategoryRequest request)
    {
        var category = await _categoryRepository.CreateCategory(request);

        return new Response<Category>(Status.Created, category, $"/api/v1/categories/{category.Id}");
    }
}