public class UpdateCategoryUseCase
{
    private readonly ICategoryRepository _categoryRepository;

    public UpdateCategoryUseCase(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<Response<Category>> Handle(int id, CreateCategoryRequest request)
    {
        try
        {
            var category = await _categoryRepository.UpdateCategory(id, request);

            return new Response<Category>(Status.OK, category);
        }
        catch (KeyNotFoundException ex)
        {
             return new Response<Category>(Status.NotFound, default, ex.Message);
        }
    }
}