using FluentAssertions;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

public class UpdateCategoryUseCaseTests
{
    private readonly ICategoryRepository _categoryRepository = Substitute.For<ICategoryRepository>();

    private readonly UpdateCategoryUseCase _useCase;

    public UpdateCategoryUseCaseTests()
    {
        // SUT
        _useCase = new UpdateCategoryUseCase(_categoryRepository);
    }

    [Fact]
    public async Task Should_Successfully_Update_Category()
    {
        // Arrange
        var request = new CreateCategoryRequest("Update Name", "");

        _categoryRepository.UpdateCategory(123, request).Returns(Task.FromResult(new Category()
        {
            Id = 123,
            Name = "Beverage",
            Description = "Sample Desc"
        }));

        // Act
        var result = await _useCase.Handle(123, request);

        // Assert
        result.Should().BeOfType<Response<Category>>();
        result.Status.Should().Be(Status.OK);
        result.Message.Should().Be("Success");
        result.Data.Should().BeEquivalentTo(new Category()
        {
            Id = 123,
            Name = "Beverage",
            Description = "Sample Desc"
        });
        await _categoryRepository.Received(1).UpdateCategory(123, request);
    }

    [Fact]
    public async Task Should_Throw_An_Error_If_Category_Does_Not_Exists()
    {
        // Arramge
        var request = new CreateCategoryRequest("Update Name", "");

        _categoryRepository.UpdateCategory(123, request).Throws(new KeyNotFoundException("Category does not exist"));

        // Act
        var result = await _useCase.Handle(123, request);

        // Assert
        result.Should().BeOfType<Response<Category>>();
        result.Status.Should().Be(Status.NotFound);
        result.Message.Should().Be("Category does not exist");
        result.Data.Should().BeNull();
        await _categoryRepository.Received(1).UpdateCategory(123, request);
    }
}