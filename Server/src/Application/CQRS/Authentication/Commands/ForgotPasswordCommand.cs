using Application.Abstractions.Messaging;

namespace Application.CQRS.Authentication.Commands;

public record ForgotPasswordCommand(string Email) : ICommand;