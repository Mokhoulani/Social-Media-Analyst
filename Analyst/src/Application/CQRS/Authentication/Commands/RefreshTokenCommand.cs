using Application.Abstractions.Messaging;
using Application.Common.Mod.ViewModels;
using Domain.Shared;
using Microsoft.AspNetCore.Authorization;


namespace Application.CQRS.Authentication.Commands;

[AllowAnonymous]
public sealed record RefreshTokenCommand(string RefreshToken) : ICommand<Result<TokenResponseViewModel>>;