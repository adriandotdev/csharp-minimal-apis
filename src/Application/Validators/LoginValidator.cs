using FluentValidation;

public class LoginValidator : AbstractValidator<LoginRequest>
{
    public LoginValidator()
    {

        RuleFor(payload => payload.Username)
            .NotEmpty()
            .WithMessage("Username is required");

        RuleFor(payload => payload.Password)
            .NotEmpty()
            .WithMessage("Password is required");
    }
}