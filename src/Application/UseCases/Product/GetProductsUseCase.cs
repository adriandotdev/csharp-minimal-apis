using Application.Interfaces;

namespace UseCase
{
    public class GetProductsUseCase
    {
        private readonly IProductRepository _productRepository;

        public GetProductsUseCase(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<Response<GetProductsResponse>> Handle(ProductFilter productFilter)
        {
            if (productFilter.PageNumber <= 0) return new Response<GetProductsResponse>(Status.BadRequest, null, $"Invalid page number value: {productFilter.PageNumber}");

            if (productFilter.PageSize <= 0) return new Response<GetProductsResponse>(Status.BadRequest, null, $"Invalid page size value: {productFilter.PageSize}");

            if (productFilter.MinPrice <= 0) return new Response<GetProductsResponse>(Status.BadRequest, null, $"Invalid minimum price value of {productFilter.MinPrice}");

            var productCount = await _productRepository.GetProductCount(productFilter);

            int pageSize = productFilter.PageSize ?? 10;
            int pageNumber = productFilter.PageNumber ?? 1;

            decimal totalPages = pageSize >= productCount ? 1 : ((decimal)productCount / pageSize)!;

            int? nextPage = ((pageNumber + 1) > Math.Ceiling(totalPages)) ? null : pageNumber + 1;

            int? previousPage = pageNumber - 1 <= 0 ? null : pageNumber - 1;

            var products = await _productRepository.GetProducts(productFilter);

            var response = new GetProductsResponse(

                products,
                new Pagination(

                    productCount,
                    pageNumber,
                    (int)Math.Ceiling(totalPages),
                    nextPage,
                    previousPage
                )
            );


            return new Response<GetProductsResponse>(Status.OK, response);
        }
    }
}