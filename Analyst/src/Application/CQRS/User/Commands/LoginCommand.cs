using Application.Abstractions.Messaging;
using Application.Common.Mod.ViewModels;
using Domain.Shared;
using Microsoft.AspNetCore.Authorization;


namespace Application.CQRS.User.Commands;

[AllowAnonymous]
public record LoginCommand(string Email, string Password) : ICommand<Result<TokenResponseViewModel>>;
