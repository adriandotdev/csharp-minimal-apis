using Application.Interfaces;
using YamlDotNet.Core.Tokens;

namespace UseCase
{
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

                return new Response<Product>(Status.OK, product);
            }
            catch (KeyNotFoundException e)
            {
                return new Response<Product>(Status.NotFound, null, e.Message);
            }
            catch (Exception e)
            {
                return new Response<Product>(Status.InternalServerError, null, e.Message);
            }
        }
    }
}