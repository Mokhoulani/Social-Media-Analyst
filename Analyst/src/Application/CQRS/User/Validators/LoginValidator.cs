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
            .EmailAddress().WithMessage("Invalid email format.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required.")
            .MinimumLength(6).WithMessage("Password cannot exceed 6 characters.");
    }
}