using Application.Abstractions.Messaging;

namespace Application.CQRS.Authentication.Commands;

public record RequestPasswordResetCommand(string Email) : ICommand;