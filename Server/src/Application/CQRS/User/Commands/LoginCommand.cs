using Application.Abstractions.Messaging;

namespace Application.CQRS.User.Commands;
public record LoginCommand(string Email, string Password) : ICommand<string>;
