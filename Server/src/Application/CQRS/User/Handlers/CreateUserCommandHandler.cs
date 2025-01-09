using Domain.Interfaces;
using Domain.ValueObjects;
using MediatR;
using Application.CQRS.User.Commands;


namespace Application.CQRS.User.Handlers;

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, int>
{
    private readonly IRepository<Domain.Entities.User> _repository;

    public CreateUserCommandHandler(IRepository<Domain.Entities.User> repository)
    {
        _repository = repository;
    }

    public async Task<int> Handle(CreateUserCommand command, CancellationToken cancellationToken)
    {
        // Validate input (optional: you might throw a custom exception or use validation libraries)
        var email = new Email(command.Email);
        // var timeZone = new Domain.ValueObjects.TimeZone(command.TimeZone);

        // Create the User Aggregate
        var user = new Domain.Entities.User(email, command.Name);

        // Persist the User Aggregate
        await _repository.AddAsync(user, cancellationToken);

        // Return the user ID
        return user.Id;
    }

}