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
        IQueryable<Product> queryBuilder = _context.Products;

        if (!string.IsNullOrWhiteSpace(productFilter.ProductName))
            queryBuilder = queryBuilder.Where(product => product.Name.ToLower().Contains(productFilter.ProductName.ToLower()));

        if (!string.IsNullOrWhiteSpace(productFilter.Category))
            queryBuilder = queryBuilder.Where(product => EF.Functions.ILike(productFilter.Category, product.Category.Name));

        if (productFilter.MinPrice.HasValue)
            queryBuilder = queryBuilder.Where(product => product.Price >= productFilter.MinPrice);

        if (productFilter.MaxPrice.HasValue)
            queryBuilder = queryBuilder.Where(product => product.Price <= productFilter.MaxPrice);

        int pageNumber = productFilter.PageNumber ?? 1;
        int pageSize = productFilter.PageSize ?? 10;

        return await queryBuilder
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
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

    public async Task<int> GetProductCount(ProductFilter productFilter)
    {
         IQueryable<Product> queryBuilder = _context.Products;

        if (!string.IsNullOrWhiteSpace(productFilter.ProductName))
            queryBuilder = queryBuilder.Where(product => product.Name.ToLower().Contains(productFilter.ProductName.ToLower()));

        if (!string.IsNullOrWhiteSpace(productFilter.Category))
            queryBuilder = queryBuilder.Where(product => EF.Functions.ILike(productFilter.Category, product.Category.Name));

        if (productFilter.MinPrice.HasValue)
            queryBuilder = queryBuilder.Where(product => product.Price >= productFilter.MinPrice);

        return await queryBuilder.CountAsync();
    }

    public async Task<Product> UpdateProductById(int id, UpdateProductRequest request)
    {
        var product = await _context.Products.FindAsync(id);

        if (product is null) throw new KeyNotFoundException($"Product with ID of {id} is not found");

        product.Name = request.Name ?? product.Name;
        product.Description = request.Description ?? product.Description;

        await _context.SaveChangesAsync();

        return product;
    }
}