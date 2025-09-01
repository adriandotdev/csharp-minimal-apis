using Application.Interfaces;
using YamlDotNet.Core.Tokens;

public class GetProductByIdUseCase
{

    private readonly IProductRepository _productRepository;

    public GetProductByIdUseCase(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<Response<Product>> Handle(int id)
    {
        try
        {
            var product = await _productRepository.GetProductById(id);

            if (product is null) return new Response<Product>(Status.NotFound, null);

            return new Response<Product>(Status.OK, product);
        }
        catch (Exception e)
        {
            return new Response<Product>(Status.Forbidden, null);
        }
    }
}