using Application.Interfaces;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

public class UpdateProductUseCaseTests
{

    private readonly IProductRepository _productReposiotry = Substitute.For<IProductRepository>();
    private readonly UpdateProductUseCase _useCase;

    private Product productToUpdate;
    private UpdateProductRequest updateProductRequest;

    public UpdateProductUseCaseTests()
    {
        _useCase = new UpdateProductUseCase(_productReposiotry);
        productToUpdate = new Product()
        {
            Name = "Coca-Cola",
            Id = 1,
            CostPrice = 10,
            Price = 12
        };
        updateProductRequest = new UpdateProductRequest("New Name", "Description", "", "", 0, 0, 0, "");
    }

    [Fact]
    public async Task Should_Successfully_Update_Product()
    {
        // Arrange
        _productReposiotry.UpdateProductById(1, updateProductRequest).Returns(Task.FromResult(productToUpdate));

        // Act
        var result = await _useCase.Handle(1, updateProductRequest);

        // Assert
        result.Should().BeOfType<Response<UpdateProductResponse>>();
        result.Status.Should().Be(Status.OK);
        result.Message.Should().Be("Success");
        result.Data.Should().BeEquivalentTo(new UpdateProductResponse(productToUpdate.Name, productToUpdate.Description!));
        await _productReposiotry.Received(1).UpdateProductById(1, updateProductRequest);
    }

    [Fact]
    public async Task Should_Throw_An_Error_If_Product_Id_Does_Not_Exists()
    {
        // Arrange
        _productReposiotry.UpdateProductById(1, updateProductRequest).Throws(new KeyNotFoundException($"Product with ID of 1 is not found"));

        // Act
        var result = await _useCase.Handle(1, updateProductRequest);

        // Assert
        result.Should().BeOfType<Response<UpdateProductResponse>>();
        result.Status.Should().Be(Status.NotFound);
        result.Message.Should().Be("Product with ID of 1 is not found");
        result.Data.Should().BeNull();
    }
}