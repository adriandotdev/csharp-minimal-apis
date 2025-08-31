using Application.Interfaces;

public class GetProductByIdUseCase
{

    private readonly IProductRepository _productRepository;

    public GetProductByIdUseCase(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<Product> Handle(int id)
    {
        return await _productRepository.GetProductById(id);
    }
}