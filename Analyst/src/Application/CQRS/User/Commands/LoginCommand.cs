using Application.Abstractions.Messaging;
using Application.Common.CustomAttributes;
using Application.Common.Mod.ViewModels;
using Domain.Enums;
using Domain.Shared;
using Microsoft.AspNetCore.Authorization;


namespace Application.CQRS.User.Commands;

[HasPermission(Permission.ReadPermission)]
public record LoginCommand(string Email, string Password) : ICommand<Result<TokenResponseViewModel>>;
