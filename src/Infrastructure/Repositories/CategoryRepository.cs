
using Microsoft.EntityFrameworkCore;

public class CategoryRepository : ICategoryRepository
{
    private readonly AppDbContext _context;

    public CategoryRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Category> CreateCategory(CreateCategoryRequest request)
    {
        Category category = new()
        {
            Name = request.Name,
            Description = request.Description,
        };

        _context.Categories.Add(category);

        await _context.SaveChangesAsync();

        return category;
    }

    public async Task<ICollection<Category>> GetCategories()
    {
        return await _context.Categories.ToListAsync();
    }

    public async Task<Category> GetCategoryById(int id)
    {
        var category = await _context.Categories.FindAsync(id);

        return category;
    }

    public Task<Category> UpdateCategory(int id, CreateCategoryRequest request)
    {
        throw new NotImplementedException();
    }
}