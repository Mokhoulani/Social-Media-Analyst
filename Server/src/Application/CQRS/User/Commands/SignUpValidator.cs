using Domain.Interfaces;
using FluentValidation;

namespace Application.CQRS.User.Commands;

public class SignUpValidator : AbstractValidator<SignUpCommand>
{
    private readonly IRepository<Domain.Entities.User> _repository;

    public SignUpValidator(IRepository<Domain.Entities.User> repository)
    {
        _repository = repository;
        
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Invalid email format.")
            .MustAsync(BeUniqueEmailAsync).WithMessage("The email is already in use.");

        RuleFor(x => x.Username)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(50).WithMessage("Name cannot exceed 50 characters.");
        
           RuleFor(x => x.Password)
               .NotEmpty().WithMessage("Password is required.")
               .MinimumLength(6).WithMessage("Password cannot exceed 6 characters.");
    }
    
    private async Task<bool> BeUniqueEmailAsync(
        string email,
        CancellationToken cancellationToken)
    {
        return !await _repository.EmailExistsAsync(email, cancellationToken);
    }
}
