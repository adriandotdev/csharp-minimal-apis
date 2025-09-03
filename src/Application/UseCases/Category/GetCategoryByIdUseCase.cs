public class GetCategoryByIdUseCase
{

    private readonly ICategoryRepository _categoryRepository;


    public GetCategoryByIdUseCase(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<Response<Category>> Handle(int id)
    {
        try
        {
            var category = await _categoryRepository.GetCategoryById(id);

            return new Response<Category>(Status.OK, category);
        }
        catch (KeyNotFoundException e)
        {
            return new Response<Category>(Status.NotFound, null, e.Message);
        }
        catch (Exception e)
        {
            return new Response<Category>(Status.InternalServerError, null, e.Message);
        }
    }
}