namespace Application.Interfaces;

public interface IProductRepository
{
    Task<ICollection<Product>> GetProducts(ProductFilter productFilter);
    Task<CreateProductResponse> CreateProduct(CreateProductRequest request);
    Task<Product> GetProductById(int id);
    Task DeleteProductById(int id);

    Task<int> GetProductCount(ProductFilter productFilter);

    Task<Product> UpdateProductById(int id, UpdateProductRequest request);
}