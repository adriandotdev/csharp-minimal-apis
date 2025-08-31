using Application.Interfaces;

public class GetProductsUseCase
{

    private readonly IProductRepository _productRepository;

    public GetProductsUseCase(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<ICollection<Product>> Handle()
    {
        return await _productRepository.GetProducts();
    }
}