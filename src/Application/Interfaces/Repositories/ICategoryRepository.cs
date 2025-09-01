public interface ICategoryRepository
{
    Task<ICollection<Category>> GetCategories();
    Task<Category> AddCategory(CreateCategoryRequest request);
    Task<Category> UpdateCategory(int id, CreateCategoryRequest request);
}