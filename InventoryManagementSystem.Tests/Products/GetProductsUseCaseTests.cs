using Application.Interfaces;
using FluentAssertions;
using NSubstitute;
using UseCase;

public class GetProductsUseCaseTest
{
    private readonly IProductRepository _productRepository = Substitute.For<IProductRepository>();

    private readonly GetProductsUseCase _useCase;

    public GetProductsUseCaseTest()
    {
        // SUT (System Under Test)
        _useCase = new GetProductsUseCase(_productRepository);
    }

    [Fact]
    public async Task Fetch_Products_Successfully()
    {
        // Arrange  
        var filter = new ProductFilter() { PageSize = 10, PageNumber = 1 };
        ICollection<Product> products = [
            new Product() {
                Id = 1,
                Name = "Cracklings",
                Price = 12,
                CostPrice = 10,
                CategoryId = 1
            },
            new Product() {
                Id = 2,
                Name = "Coca-Cola",
                Price = 15,
                CostPrice = 10,
                CategoryId = 2
            }
        ];
        _productRepository.GetProductCount(filter).Returns(Task.FromResult(2));
        _productRepository.GetProducts(filter).Returns(Task.FromResult(products));

        // Act
        var results = await _useCase.Handle(filter);

        // Assert
        results.Should().BeOfType<Response<GetProductsResponse>>();
        results.Status.Should().Be(Status.OK);
        results.Data.Should().NotBeNull();
        results.Data.products.Should().HaveCount(2);
        results.Data.pagination.NextPage.Should().BeNull();
        results.Data.pagination.PrevPage.Should().BeNull();
        results.Data.pagination.TotalPages.Should().Be(1);
        results.Data.pagination.CurrentPage.Should().Be(1);

        results.Should().BeEquivalentTo(
           new Response<GetProductsResponse>(
                Status.OK,
                new GetProductsResponse(
                    products,
                    new Pagination(2, 1, 1, null, null)
            ))
        );
    }

    [Fact]
    public async Task Should_Properly_Return_Next_Page_Value()
    {
        // Arrange  
        var filter = new ProductFilter() { PageSize = 10, PageNumber = 1 };
        ICollection<Product> products = [];

        for (int i = 1; i <= 15; i++)
        {
            products.Add(new Product()
            {
                Id = 2,
                Name = "Coca-Cola",
                Price = 15,
                CostPrice = 10,
                CategoryId = 2
            });
        }

        ICollection<Product> returningProducts = (ICollection<Product>)products.Skip(1).Take(10);

        _productRepository.GetProductCount(filter).Returns(Task.FromResult(15));
        _productRepository.GetProducts(filter).Returns(Task.FromResult(returningProducts));

        // Act
        var result = await _useCase.Handle(filter);

        // Assert
        result.Should().BeOfType<Response<GetProductsResponse>>();
        result.Data.Should().NotBeNull();
        result.Data.products.Should().HaveCount(10); // This should verify the paginated result count
        result.Data.pagination.Should().NotBeNull();
        result.Data.pagination.NextPage.Should().NotBeNull().And.Be(2);
        result.Data.pagination.CurrentPage.Should().Be(1);
        result.Data.pagination.TotalPages.Should().Be(2);
    }

    [Fact]
    public async Task Should_Properly_Return_Second_Page()
    {
        // Arrange
        var filter = new ProductFilter() { PageSize = 10, PageNumber = 2 };
        ICollection<Product> products = [];

        for (int i = 1; i <= 15; i++)
        {
            products.Add(new Product()
            {
                Id = 2,
                Name = "Coca-Cola",
                Price = 15,
                CostPrice = 10,
                CategoryId = 2
            });
        }

        ICollection<Product> returningProducts = (ICollection<Product>)products.Skip(10).Take(5);

        _productRepository.GetProductCount(filter).Returns(Task.FromResult(15));
        _productRepository.GetProducts(filter).Returns(Task.FromResult(returningProducts));

        // Act
        var result = await _useCase.Handle(filter);

        // Assert
        result.Should().BeOfType<Response<GetProductsResponse>>();
        result.Data.Should().NotBeNull();
        result.Data.products.Should().HaveCount(5);
        result.Data.pagination.CurrentPage.Should().Be(2);
        result.Data.pagination.PrevPage.Should().Be(1);
        result.Data.pagination.NextPage.Should().BeNull();
    }

    [Fact]
    public async Task Should_Return_Bad_Request_Status_When_PageNumber_Is_Invalid()
    {
        // Arrange
        var productFilter = new ProductFilter() { PageNumber = 0 };

        // Act
        var result = await _useCase.Handle(productFilter);

        // Assert
        result.Status.Should().Be(Status.BadRequest);
        result.Message.Should().Be("Invalid page number value: 0");
        result.Data.Should().BeNull();
    }


    [Fact]
    public async Task Should_Return_Bad_Request_When_PageSize_Is_Invalid()
    {
        // Arrange
        var productFilter = new ProductFilter() { PageSize = 0 };

        // Act
        var result = await _useCase.Handle(productFilter);

        // Assert
        result.Status.Should().Be(Status.BadRequest);
        result.Message.Should().Be("Invalid page size value: 0");
        result.Data.Should().BeNull();
    }

    [Fact]
    public async Task Should_Return_Bad_Request_When_MinPrice_Is_Invalid()
    {
        // Arrange
        var productFilter = new ProductFilter() { MinPrice = 0 };

        // Act
        var result = await _useCase.Handle(productFilter);

        // Assert
        result.Status.Should().Be(Status.BadRequest);
        result.Message.Should().Be("Invalid minimum price value of 0");
        result.Data.Should().BeNull();
    }
}