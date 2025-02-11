using Application.Abstractions.Messaging;
using Application.Common.Mod;


namespace Application.CQRS.User.Commands;
public record LoginCommand(string Email, string Password) : ICommand<TokenResponse>;
