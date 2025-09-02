public interface ICategoryRepository
{
    Task<ICollection<Category>> GetCategories();
    Task<Category> CreateCategory(CreateCategoryRequest request);
    Task<Category> UpdateCategory(int id, CreateCategoryRequest request);

    Task<Category> GetCategoryById(int id);
}