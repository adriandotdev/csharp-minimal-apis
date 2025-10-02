using Application.Interfaces;

namespace UseCase
{
    public class CreateProductUseCase
    {

        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;


        public CreateProductUseCase(IProductRepository productRepository, ICategoryRepository categoryRepository)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
        }

        public async Task<Response<CreateProductResponse>> Handle(CreateProductRequest request)
        {
            try
            {
                await _categoryRepository.GetCategoryById(request.CategoryId);

                var createdProduct = await _productRepository.CreateProduct(request);
                return new Response<CreateProductResponse>(Status.Created, createdProduct, $"/api/v1/products/{createdProduct.Id}");

            }
            catch (KeyNotFoundException ex)
            {
                return new Response<CreateProductResponse>(Status.NotFound, null, ex.Message);
            }
        }
    }
}