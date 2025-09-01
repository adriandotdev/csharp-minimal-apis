
using Microsoft.EntityFrameworkCore;

public class CategoryRepository : ICategoryRepository
{
    private readonly AppDbContext _context;

    public CategoryRepository(AppDbContext context)
    {
        _context = context;
    }

    public Task<Category> AddCategory(CreateCategoryRequest request)
    {
        throw new NotImplementedException();
    }

    public async Task<ICollection<Category>> GetCategories()
    {
        return await _context.Categories.ToListAsync();
    }

    public Task<Category> UpdateCategory(int id, CreateCategoryRequest request)
    {
        throw new NotImplementedException();
    }
}