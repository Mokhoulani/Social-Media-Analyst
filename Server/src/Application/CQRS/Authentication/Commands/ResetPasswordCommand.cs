using Application.Abstractions.Messaging;

namespace Application.CQRS.Authentication.Commands;

public record ResetPasswordCommand(string Token, string NewPassword) : ICommand<bool>;