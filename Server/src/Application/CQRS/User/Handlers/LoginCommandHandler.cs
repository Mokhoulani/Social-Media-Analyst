using Application.Abstractions;
using Application.Abstractions.Messaging;
using Application.CQRS.User.Commands;
using Domain.Interfaces;
using Domain.ValueObjects;

namespace Application.CQRS.User.Handlers;

internal sealed class LoginCommandHandler(
    IUserRepository userRepository,
    IJwtProvider jwtProvider)
    : ICommandHandler<LoginCommand, string>
{
    public async Task<string> Handle(
        LoginCommand request,
        CancellationToken cancellationToken)
    {
        Email email = Email.Create(request.Email);

      Domain.Entities.User? user = await userRepository.GetByEmailAsync(
            email,
            cancellationToken);
      
        string token = jwtProvider.Generate(user);

        return token;
    }
}
