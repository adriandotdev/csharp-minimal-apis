using Application.Interfaces;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

public class DeleteProductByIdUseCaseTest
{
    private readonly IProductRepository _productRepository = Substitute.For<IProductRepository>();

    private readonly DeleteProductByIdUseCase _useCase;

    public DeleteProductByIdUseCaseTest()
    {
        _useCase = new DeleteProductByIdUseCase(_productRepository);
    }

    [Fact]
    public async Task Should_Successfully_Delete_A_Product()
    {
        // Arrange
        _productRepository.GetProductById(1).Returns(Task.FromResult(new Product()
        {
            Name = "Coca-Cola",
            Id = 1,
            CostPrice = 10,
            Price = 12
        }));
        _productRepository.DeleteProductById(1).Returns(Task.CompletedTask);
        var message = "Product with ID of 1 is successfully deleted";

        // Act
        var result = await _useCase.Handle(1);

        // Assert
        result.Should().BeOfType<Response<object>>();
        result.Status.Should().Be(Status.OK);
        result.Message.Should().Be(message);
    }

    [Fact]
    public async Task Should_Return_Not_Found_When_Product_Does_Not_Exists()
    {
        // Arrange
        var message = "Product with ID of 1 is not found";
        _productRepository.GetProductById(1).Throws(new KeyNotFoundException(message));

        // Act
        var result = await _useCase.Handle(1);

        // Assert
        result.Should().BeOfType<Response<object>>();
        result.Status.Should().Be(Status.NotFound);
        result.Message.Should().Be(message);
        result.Data.Should().BeNull();
    }
}