public interface ICategoryRepository
{
    Task<ICollection<GetCategoriesResponse>> GetCategories();
    Task<Category> CreateCategory(CreateCategoryRequest request);
    Task<Category> UpdateCategory(int id, CreateCategoryRequest request);

    Task<Category> GetCategoryById(int id);
}