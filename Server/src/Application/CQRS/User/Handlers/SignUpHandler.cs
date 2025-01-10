using Application.Common.Modoles.ViewModels;
using Domain.Interfaces;
using MediatR;
using Application.CQRS.User.Commands;
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
        
        var user = new Domain.Entities.User
        {
            UserName = command.Username,
            Email = command.Email
        };
        
        await repository.AddAsync(user, cancellationToken);
        
        return mapper.Map<AppUserViewModel>(user);
    }
  
}

