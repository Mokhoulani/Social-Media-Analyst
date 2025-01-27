using Application.Abstractions;
using Application.Abstractions.Messaging;
using Application.Common.Interfaces;
using Application.CQRS.User.Commands;
using Domain.Interfaces;
using Domain.ValueObjects;

namespace Application.CQRS.User.Handlers;

internal sealed class LoginCommandHandler(
    IUserService userService,
    IJwtProvider jwtProvider)
    : ICommandHandler<LoginCommand, string>
{
    public async Task<string> Handle(
        LoginCommand request,
        CancellationToken cancellationToken)
    {
        var user = await userService.LoginAsync(request, cancellationToken);

        string token = jwtProvider.Generate(user!);

        return token;
    }
}
