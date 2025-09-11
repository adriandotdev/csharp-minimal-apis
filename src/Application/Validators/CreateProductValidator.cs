using FluentValidation;

public class CreateProductValidator : AbstractValidator<CreateProductRequest>
{
    public CreateProductValidator()
    {
        RuleFor(product => product.Name)
            .NotEmpty()
            .WithMessage("Product name is required");

        RuleFor(product => product.CategoryId)
            .NotEmpty()
            .NotNull()
            .WithMessage("Product category must be provided");

        RuleFor(product => product.Price)
            .NotEmpty()
            .NotNull()
            .WithMessage("Product price is required");

        RuleFor(product => product.CostPrice)
            .NotEmpty()
            .NotNull()
            .WithMessage("Product cost price is required");
    }
}