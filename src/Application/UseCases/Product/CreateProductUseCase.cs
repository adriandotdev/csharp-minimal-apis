using Application.Interfaces;

public class CreateProductUseCase
{

    private readonly IProductRepository _productRepository;

    public CreateProductUseCase(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<Response<Product>> Handle(CreateProductRequest request)
    {
       return new Response<Product>(Status.Created, await _productRepository.CreateProduct(request));
    }
}