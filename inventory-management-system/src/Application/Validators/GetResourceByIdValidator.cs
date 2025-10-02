using FluentValidation;

public class GetResourceByIdValidator : AbstractValidator<IdRequest>
{
    public GetResourceByIdValidator()
    {
        RuleFor(id => id.Id)
            .NotEmpty()
            .WithMessage("Resource ID must be provided")
            .Must(id => int.TryParse(id, out _))
            .WithMessage("Resource ID must be on type of Integer");
    }
}