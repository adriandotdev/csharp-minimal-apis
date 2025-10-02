using System.Net.Mail;
using FluentValidation;

public class CreateUserValidator : AbstractValidator<CreateUserRequest>
{
    public CreateUserValidator()
    {
        RuleFor(user => user.Name)
            .NotEmpty()
            .WithMessage("Name is required");

        RuleFor(user => user.Username)
            .NotEmpty()
            .WithMessage("Username is required");

        RuleFor(user => user.Email)
            .Matches(@"^[^@\s]+@[^@\s]+\.[^@\s]+$")
            .WithMessage("Email is not valid");

        RuleFor(user => user.Password)
            .NotEmpty()
            .WithMessage("Password is required")
            .MaximumLength(8)
            .WithMessage("Password must be at least eight (8) characters");

        RuleFor(user => user.Role)
            .NotEmpty()
            .WithMessage("Role is required")
            .Must(value => Enum.TryParse<UserRoles>(value, true, out _))
            .WithMessage($"Role must be one of: {string.Join(", ", Enum.GetNames<UserRoles>())}");

        RuleFor(user => user.Status)
            .NotEmpty()
            .WithMessage("Status is required")
            .Must(value => Enum.TryParse<UserStatuses>(value, true, out _))
            .WithMessage($"Role must be one of: {string.Join(", ", Enum.GetNames<UserStatuses>())}");
    
    }
}