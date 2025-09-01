using Application.Interfaces;
using Microsoft.EntityFrameworkCore;

public class ProductRepository : IProductRepository
{
    private readonly AppDbContext _context;

    public ProductRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<ICollection<Product>> GetProducts()
    {
        return await _context.Products.ToListAsync();
    }

    public async Task<Product> CreateProduct(CreateProductRequest request)
    {
        Product product = new()
        {
            Name = request.Name,
            Description = request.Description,
            SKU = request.SKU,
            BarCode = request.BarCode,
            CategoryId = request.CategoryId,
            Price = request.Price,
            CostPrice = request.Price,
            ImageUrl = request.ImageUrl,
        };

        this._context.Add(product);

        await _context.SaveChangesAsync();

        return product;
    }

    public async Task<Product> GetProductById(int id)
    {
        var product = await _context.Products.FindAsync(id);

        return product;
    }

    public async Task DeleteProductById(int id)
    {
        var product = await _context.Products.FindAsync(id);

        if (product is not null)
        {
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
        }
    }
}