using Application.Interfaces;

public class UpdateProductUseCase
{

    private readonly IProductRepository _productRepository;

    public UpdateProductUseCase(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<Response<UpdateProductResponse>> Handle(int id, UpdateProductRequest request)
    {
        try
        {
            var product = await _productRepository.UpdateProductById(id, request);

            return new Response<UpdateProductResponse>(Status.OK,
            new UpdateProductResponse(product.Name, product.Description!));
        }
        catch (KeyNotFoundException ex)
        {
            return new Response<UpdateProductResponse>(Status.NotFound, null, ex.Message);
        }
    }
}