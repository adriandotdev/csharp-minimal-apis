public class GetCategoryByIdUseCase
{

    private readonly ICategoryRepository _categoryRepository;


    public GetCategoryByIdUseCase(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<Response<Category>> Handle(int id)
    {
        var category = await _categoryRepository.GetCategoryById(id);

        if (category is null) return new Response<Category>(Status.NotFound, default);

        return new Response<Category>(Status.OK, category);
    }
}