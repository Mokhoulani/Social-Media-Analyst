using Application.Abstractions.Messaging;
using Application.Common.Mod.ViewModels;
using Domain.Shared;

namespace Application.CQRS.Authentication.Commands;

public record ResetPasswordCommand(string Token, string NewPassword) : ICommand<Result<PasswordResetViewModel>>;