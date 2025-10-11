using FluentAssertions;
using NSubstitute;

public class GetCategoriesUseCaseTests
{
    private readonly ICategoryRepository _categoryRepository = Substitute.For<ICategoryRepository>();

    private readonly GetCategoriesUseCase _useCase;

    public GetCategoriesUseCaseTests()
    {
        // SUT (System Under Tests)
        _useCase = new GetCategoriesUseCase(_categoryRepository);
    }

    [Fact]
    public async Task Should_Successfully_Retrieve_Categories()
    {
        // Arramge
        _categoryRepository.GetCategories().Returns([new GetCategoriesResponse(1, "Beverage", "Desc 1")]);

        // Act
        var result = await _useCase.Handle();

        // Assert
        result.Should().BeOfType<Response<ICollection<GetCategoriesResponse>>>();
        result.Status.Should().Be(Status.OK);
        result.Message.Should().Be("Success");
        await _categoryRepository.Received().GetCategories();
    }
}