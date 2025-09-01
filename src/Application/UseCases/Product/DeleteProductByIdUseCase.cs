using Application.Interfaces;

public class DeleteProductByIdUseCase
{

    private readonly IProductRepository _productRepository;

    public DeleteProductByIdUseCase(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<Response<object?>> Handle(int id)
    {
        try
        {
            var product = await _productRepository.GetProductById(id);

            if (product is null) return new Response<object?>(Status.NotFound, null,  message: $"Product with ID of {id} is not found");

            await _productRepository.DeleteProductById(id);

            return new Response<object?>(Status.OK, null, $"Product with ID of {id} is successfully deleted");
        }
        catch (Exception e)
        {
            return new Response<object?>(Status.InternalServerError, "Internal Server Error");
        }
    }
}