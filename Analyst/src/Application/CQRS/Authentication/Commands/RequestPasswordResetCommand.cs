using Application.Abstractions.Messaging;
using Application.Common.Mod.ViewModels;
using Domain.Shared;

namespace Application.CQRS.Authentication.Commands;

public record RequestPasswordResetCommand(string Email) : ICommand<Result<PasswordResetViewModel>>;


