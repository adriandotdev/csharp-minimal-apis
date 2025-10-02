using Application.Interfaces;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using UseCase;

public class CreateProductUseCaseTests
{
    private readonly IProductRepository _productRepository = Substitute.For<IProductRepository>();
    private readonly ICategoryRepository _categoryRepository = Substitute.For<ICategoryRepository>();

    private readonly CreateProductUseCase _useCase;

    public CreateProductUseCaseTests()
    {
        // SUT (System Under Tests)
        _useCase = new CreateProductUseCase(_productRepository, _categoryRepository);
    }

    [Fact]
    public async Task Should_Successfully_Create_New_Product()
    {
        // Arrange
        var request = new CreateProductRequest("Cracklings", "", "", "", 1, 12, 10, "");

        var response = new CreateProductResponse(1, request.Name, request.Description ?? "", request.CategoryId, request.Price, request.CostPrice);

        _categoryRepository.GetCategoryById(1).Returns(new Category() { });
        _productRepository.CreateProduct(request).Returns(response);

        // Act
        var result = await _useCase.Handle(request);

        // Assert
        result.Should().BeOfType<Response<CreateProductResponse>>();
        result.Status.Should().Be(Status.Created);
        result.Data.Should().NotBeNull();
        result.Data.Should().BeEquivalentTo(response);
    }

    [Fact]
    public async Task Should_Return_Not_Found_When_Category_Is_Not_Existing()
    {
        // Arrange
        var request = new CreateProductRequest("Cracklings", "", "", "", 1, 12, 10, "");
        var message = "Category with ID of 1 not found";
        _categoryRepository.GetCategoryById(1).Throws(new KeyNotFoundException(message));

        // Act
        var result = await _useCase.Handle(request);

        // Assert
        result.Should().BeOfType<Response<CreateProductResponse>>();
        result.Status.Should().Be(Status.NotFound);
        result.Data.Should().BeNull();
        result.Message.Should().Be(message);
    }
}