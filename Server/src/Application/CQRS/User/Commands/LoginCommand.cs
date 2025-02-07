using Application.Abstractions.Messaging;
using Application.Common.Mod;
using Application.Common.Mod.ViewModels;

namespace Application.CQRS.User.Commands;
public record LoginCommand(string Email, string Password) : ICommand<TokenResponse>;
