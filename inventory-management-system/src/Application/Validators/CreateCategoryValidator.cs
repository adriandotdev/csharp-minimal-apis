using FluentValidation;

public class CreateCategoryValidator : AbstractValidator<CreateCategoryRequest>
{
    public CreateCategoryValidator()
    {
        RuleFor(category => category.Name)
            .NotEmpty()
            .WithMessage("Category name is required");

        RuleFor(category => category.Description)
            .NotEmpty()
            .WithMessage("Description is required");
    }
}