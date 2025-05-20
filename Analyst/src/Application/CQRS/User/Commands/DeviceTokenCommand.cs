using Application.Abstractions.Messaging;
using Application.Common.Mod.ViewModels;
using Domain.Shared;

namespace Application.CQRS.User.Commands;

public record DeviceTokenCommand(string DeviceToken, string DeviceId) : ICommand<Result<UserDeviceViewModel>>;
