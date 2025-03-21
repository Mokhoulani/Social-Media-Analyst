using Application.Abstractions.Messaging;
using Application.Common.Mod;
using Domain.Shared;


namespace Application.CQRS.User.Commands;
public record LoginCommand(string Email, string Password) : ICommand<Result<TokenResponse>>;
