using Application.Interfaces;

public class GetProductsUseCase
{

    private readonly IProductRepository _productRepository;

    public GetProductsUseCase(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<Response<ICollection<Product>>> Handle()
    {
        return new Response<ICollection<Product>>(Status.OK, await _productRepository.GetProducts());
    }
}