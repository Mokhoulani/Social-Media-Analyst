using Application.Common.Interfaces;
using Application.CQRS.User.Commands;
using Domain.ValueObjects;
using FluentValidation;

namespace Application.CQRS.User.Validators;

public class LoginValidator : AbstractValidator<LoginCommand>
{
    private readonly IUserService _userService;

    public LoginValidator(IUserService userService)
    {
        _userService = userService;

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Invalid email format.")
            .MustAsync(BeUniqueEmailAsync).WithMessage("The email is not use.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required.")
            .MinimumLength(6).WithMessage("Password cannot exceed 6 characters.")
           .MustAsync(async (model, password, cancellationToken) =>
            {
                return await IsPasswordValidAsync(model.Email, password, cancellationToken);
            }).WithMessage("Invalid password.");
    }

    private async Task<bool> BeUniqueEmailAsync(
        string email,
        CancellationToken cancellationToken)
    {
        Email emailResult = Email.Create(email);

        return !await _userService.IsEmailExistsAsync(emailResult, cancellationToken);
    }

    private async Task<bool> IsPasswordValidAsync(
        string email,
        string password,
        CancellationToken cancellationToken)
    {
        return await _userService.IsPasswordValidAsync(email, password, cancellationToken);
    }
}