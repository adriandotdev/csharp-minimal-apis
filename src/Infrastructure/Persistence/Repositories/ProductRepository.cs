using Application.Interfaces;
using Microsoft.EntityFrameworkCore;

public class ProductRepository : IProductRepository
{
    private readonly AppDbContext _context;
    private readonly ILogger<ProductRepository> _logger;

    public ProductRepository(AppDbContext context, ILogger<ProductRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<ICollection<Product>> GetProducts(ProductFilter productFilter)
    {

        IQueryable<Product> query = _context.Products;

        if (!string.IsNullOrWhiteSpace(productFilter.ProductName))
            query = query.Where(product => product.Name.ToLower().Contains(productFilter.ProductName.ToLower()));

        if (!string.IsNullOrWhiteSpace(productFilter.Category))
            query = query.Where(product => EF.Functions.ILike(productFilter.Category, product.Category.Name));

        return await query.ToListAsync();
    }

    public async Task<CreateProductResponse> CreateProduct(CreateProductRequest request)
    {
        Product product = new()
        {
            Name = request.Name,
            Description = request.Description,
            SKU = request.SKU,
            BarCode = request.BarCode,
            CategoryId = request.CategoryId,
            Price = request.Price,
            CostPrice = request.CostPrice,
            ImageUrl = request.ImageUrl,
        };

        _logger.LogInformation($"Request: {product.CategoryId}");

        this._context.Add(product);

        await _context.SaveChangesAsync();

        _logger.LogInformation($"{product.Id}, {product.CategoryId}");
        return new CreateProductResponse(product.Id, product.Name, product.Description!, product.CategoryId, product.Price, product.CostPrice);
    }

    public async Task<Product> GetProductById(int id)
    {
        var product = await _context.Products.FindAsync(id);

        if (product is null) throw new KeyNotFoundException($"Product with id of {id} not found");
        
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