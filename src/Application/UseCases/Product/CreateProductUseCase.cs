using Application.Interfaces;

public class CreateProductUseCase
{

    private readonly IProductRepository _productRepository;

    public CreateProductUseCase(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<Product> Handle(CreateProductRequest request)
    {
       return await _productRepository.CreateProduct(request);
    }
}