using Application.Interfaces;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using UseCase;

public class GetProductByIdUseCaseTests
{
    private readonly IProductRepository _productRepository = Substitute.For<IProductRepository>();

    private readonly GetProductByIdUseCase _useCase;

    public GetProductByIdUseCaseTests()
    {
        _useCase = new GetProductByIdUseCase(_productRepository);
    }

    [Fact]
    public async Task Should_Successfully_Retrieve_A_Product_By_Id()
    {
        // Arrage
        var product = new Product()
        {
            Name = "Coca-Cola",
            Id = 1,
            CostPrice = 10,
            Price = 12
        };
        _productRepository.GetProductById(1).Returns(Task.FromResult(product));


        // Act
        var result = await _useCase.Handle(1);

        // Assert
        result.Should().BeOfType<Response<Product>>();
        result.Status.Should().Be(Status.OK);
        result.Data.Should().BeEquivalentTo(product);
    }

    [Fact]
    public async Task Should_Throw_An_Error_When_Product_Id_Does_Not_Exists()
    {
        // Arrange
        var keyNotFoundMessage = $"Product with ID of {1} is not found";
        _productRepository.GetProductById(1).Throws(new KeyNotFoundException(keyNotFoundMessage));

        // Act
        var result = await _useCase.Handle(1);

        // Assert
        result.Should().BeOfType<Response<Product>>();
        result.Status.Should().Be(Status.NotFound);
        result.Message.Should().Be(keyNotFoundMessage);
        result.Data.Should().BeNull();

        await _productRepository.Received(1).GetProductById(1);
    }
}