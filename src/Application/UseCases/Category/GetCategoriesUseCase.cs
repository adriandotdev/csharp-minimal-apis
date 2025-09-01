public class GetCategoriesUseCase
{

    private readonly ICategoryRepository _categoryRepository;

    public GetCategoriesUseCase(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<Response<ICollection<Category>>> Handle()
    {
        return new Response<ICollection<Category>>(Status.OK, await _categoryRepository.GetCategories());
    }
}