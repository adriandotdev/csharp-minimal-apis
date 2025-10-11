using FluentAssertions;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

public class GetCategoryByIdUseCaseTests
{
    private readonly ICategoryRepository _categoryRepository = Substitute.For<ICategoryRepository>();

    private readonly GetCategoryByIdUseCase _useCase;

    public GetCategoryByIdUseCaseTests()
    {
        // SUT
        _useCase = new GetCategoryByIdUseCase(_categoryRepository);
    }

    [Fact]
    public async Task Should_Successfully_Retrieve_Category_By_Id()
    {
        // Arrange
        var category = new Category()
        {
            Id = 123,
            Name = "Beverage",
            Description = "Sample Desc",
        };

        _categoryRepository.GetCategoryById(123).Returns(category);

        // Act
        var result = await _useCase.Handle(123);

        // Assert
        result.Should().BeOfType<Response<Category>>();
        result.Status.Should().Be(Status.OK);
        result.Data.Should().BeEquivalentTo(category);
        result.Message.Should().Be("Success");
        await _categoryRepository.Received(1).GetCategoryById(123);
    }

    [Fact]
    public async Task Should_Throw_An_Error_If_Category_Does_Not_Exists()
    {

        // Arrange
        _categoryRepository.GetCategoryById(123).Throws(new KeyNotFoundException("Category not found"));

        // Act
        var result = await _useCase.Handle(123);

        // Assert
        result.Message.Should().Be("Category not found");
        result.Status.Should().Be(Status.NotFound);
        result.Data.Should().BeNull();
        await _categoryRepository.Received(1).GetCategoryById(123);
    }
}