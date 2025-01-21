using Application.Common.Interfaces;
using FluentValidation;
using Domain.ValueObjects;

namespace Application.CQRS.User.Commands;

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
    }
    
    private async Task<bool> BeUniqueEmailAsync(
        string email,
        CancellationToken cancellationToken)
    {
        Email emailResult = Email.Create(email);

        return !await _userService.IsEmailExistsAsync(emailResult, cancellationToken);
    }
}