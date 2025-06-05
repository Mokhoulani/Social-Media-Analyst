using Application.Abstractions.Messaging;
using Application.Common.Mod.ViewModels;
using Domain.Shared;

namespace Application.CQRS.User.Commands;

public record GetUserCommand : ICommand<Result<AppUserViewModel>>;
