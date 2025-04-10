using Application.Common.Interfaces;
using Application.CQRS.User.Commands;
using Domain.ValueObjects;
using FluentValidation;

namespace Application.CQRS.User.Validators;

public class SignUpValidator : AbstractValidator<SignUpCommand>
{
    private readonly IUserService _userService;

    public SignUpValidator(IUserService userService)
    {
        _userService = userService;
        
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Invalid email format.")
            .MustAsync(BeUniqueEmailAsync).WithMessage("The email is already in use.");

        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("First Name is required.")
            .MaximumLength(50).WithMessage("Name cannot exceed 50 characters.");
        
        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Last Name is required.")
            .MaximumLength(50).WithMessage("Name cannot exceed 50 characters.");
        
        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required.")
            .MinimumLength(6).WithMessage("Password cannot exceed 6 characters.");
    }
    
    private async Task<bool> BeUniqueEmailAsync(
        string email,
        CancellationToken cancellationToken)
    {
       var emailResult = Email.Create(email);

        if (!emailResult.IsSuccess)
        {
            return true; 
        }

        var existsResult = await _userService.IsEmailExistsAsync(emailResult.Value, cancellationToken);
         return existsResult.IsSuccess && !existsResult.Value;
    }
}
