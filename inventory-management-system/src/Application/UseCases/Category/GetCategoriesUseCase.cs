public class GetCategoriesUseCase
{
    private readonly ICategoryRepository _categoryRepository;

    public GetCategoriesUseCase(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<Response<ICollection<GetCategoriesResponse>>> Handle()
    {
        return new(Status.OK, await _categoryRepository.GetCategories());
    }           
}