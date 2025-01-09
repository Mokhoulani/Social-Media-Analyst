using Application.Common.Exception;
using Application.Common.Modoles.ViewModels;
using Domain.Interfaces;

using MediatR;
using Application.CQRS.User.Commands;
using Domain.Exceptions;
using FluentValidation;
using MapsterMapper;


namespace Application.CQRS.User.Handlers;

public class SignUpHandler(
    IRepository<Domain.Entities.User> repository,
    IMapper mapper,
    IValidator<SignUpCommand> validator)
    : IRequestHandler<SignUpCommand, AppUserViewModel>
{
    public async Task<AppUserViewModel> Handle(
        SignUpCommand command, 
        CancellationToken cancellationToken)
    {
        await ValidateCommand(command, cancellationToken);
        
        var user = new Domain.Entities.User
        {
            UserName = command.Username,
            Email = command.Email
        };
        
        await repository.AddAsync(user, cancellationToken);
        
        return mapper.Map<AppUserViewModel>(user);
    }
    private async Task ValidateCommand(SignUpCommand command, CancellationToken cancellationToken)
    {
        var validationResult = await validator.ValidateAsync(command, cancellationToken);
        
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }
    }
}

