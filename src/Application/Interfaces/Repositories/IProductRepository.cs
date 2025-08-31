namespace Application.Interfaces;

public interface IProductRepository
{
    Task<ICollection<Product>> GetProducts();
    Task<Product> CreateProduct(CreateProductRequest request);

    Task<Product> GetProductById(int id);
}