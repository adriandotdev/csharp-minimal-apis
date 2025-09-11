using FluentValidation;

public class GetProductByIdValidator : AbstractValidator<IdRequest>
{
    public GetProductByIdValidator()
    {
        RuleFor(id => id.Id)
            .Must(id => int.TryParse(id, out _))
            .WithMessage("Product ID must be provided");
    }
}