using FluentAssertions;
using NSubstitute;

public class CreateCategoryUseCaseTests
{
    private readonly ICategoryRepository _categoryRepository = Substitute.For<ICategoryRepository>();

    private readonly CreateCategoryUseCase _useCase;

    public CreateCategoryUseCaseTests()
    {
        // SUT (System Under Tests)
        _useCase = new CreateCategoryUseCase(_categoryRepository);
    }

    [Fact]
    public async Task Should_Successfully_Create_New_Category()
    {
        // Arrange
        var request = new CreateCategoryRequest("Beverage", "This is a sample description");

        _categoryRepository.CreateCategory(request).Returns(new Category());

        // Act
        var result = await _useCase.Handle(request);

        // Assert
        result.Should().BeOfType<Response<Category>>();
        result.Status.Should().Be(Status.Created);
        result.Message.Should().Be("/api/v1/categories/0");
    }
}