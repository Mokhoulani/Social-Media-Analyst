using Application.Abstractions;
using Application.Abstractions.Messaging;
using Application.CQRS.User.Commands;
using Domain.Errors;
using Domain.Interfaces;
using Domain.Shared;
using Domain.ValueObjects;

namespace Application.CQRS.User.Handlers;

internal sealed class LoginCommandHandler(
    IUserRepository userRepository,
    IJwtProvider jwtProvider)
    : ICommandHandler<LoginCommand, string>
{
    public async Task<Result<string>> Handle(
        LoginCommand request,
        CancellationToken cancellationToken)
    {
        Result<Email> email = Email.Create(request.Email);

      Domain.Entities.User? user = await userRepository.GetByEmailAsync(
            email.Value,
            cancellationToken);

        if (user is null)
        {
            return Result.Failure<string>(
                DomainErrors.User.InvalidCredentials);
        }

        string token = jwtProvider.Generate(user);

        return token;
    }
}
