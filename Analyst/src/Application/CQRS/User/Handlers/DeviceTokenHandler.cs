using Application.Abstractions.Messaging;
using Application.Common.Interfaces;
using Application.Common.Mod.ViewModels;
using Application.CQRS.User.Commands;
using Domain.Entities;
using Domain.Errors;
using Domain.Interfaces;
using Domain.Shared;
using Domain.ValueObjects;
using MapsterMapper;

namespace Application.CQRS.User.Handlers;

internal sealed class DeviceTokenHandler(
    IUserService userService,
    ICurrentUser currentUser,
    IMapper mapper,
    IUserDeviceService userDeviceService)
    : ICommandHandler<DeviceTokenCommand, UserDeviceViewModel>
{
    public async Task<Result<UserDeviceViewModel>> Handle(
        DeviceTokenCommand command,
        CancellationToken cancellationToken)
    {
        var email = currentUser.Email;

        if (string.IsNullOrWhiteSpace(email)) return DomainErrors.User.NotFound;

        var emailResult = Email.Create(email);
        if (emailResult.IsFailure) return emailResult.Error;


        var userResult = await userService.GetByEmailAsync(emailResult.Value, cancellationToken);
        if (userResult.IsFailure) return userResult.Error;
        var userDevice = UserDevice.Create(command.DeviceToken, command.DeviceId, userResult.Value.Id);

        var userDeviceResult = await userDeviceService.CreateOrUpdateUserDevice(userDevice, cancellationToken);

        return mapper.Map<UserDeviceViewModel>(userDeviceResult.Value);
    }
}