using Application.Abstractions.Messaging;
using Application.Common.Interfaces;
using Application.Common.Mod.ViewModels;
using Application.CQRS.Usage.Commands;
using Domain.Entities;
using Domain.Shared;
using MapsterMapper;
using Microsoft.Extensions.Logging;

namespace Application.CQRS.Usage.Handlers;

public sealed class CreateOrUpdateUserSocialMediaUsageHandler(
    ILogger<CreateOrUpdateUserSocialMediaUsageHandler> logger,
    IMapper mapper,
    IUserSocialMediaUsageService service)
    : ICommandHandler<CreateOrUpdateUserSocialMediaUsageCommand,
        UserSocialMediaUsageViewModel>
{
    public async Task<Result<UserSocialMediaUsageViewModel>> Handle(
        CreateOrUpdateUserSocialMediaUsageCommand command,
        CancellationToken cancellationToken)
    {
        var userIdGuid = Guid.Parse(command.UserId);
        var startTime = DateTime.Parse(command.StartTime);

        var usgae = UserSocialMediaUsage.Create(
            command.Id,
            userIdGuid,
            command.PlatformId,
            startTime
        );

        var usgaeResult = await service.CreateOrUpdateUserSocialMediaUsageAsync(
            usgae,
            cancellationToken);

        if (usgaeResult.IsFailure)
        {
            logger.LogWarning("Failed to create or update usgae: {Error}",
                usgaeResult.Error);
            return usgaeResult.Error;
        }

        var viewModel = mapper.Map<UserSocialMediaUsageViewModel>(
            usgaeResult.Value);
        return viewModel;
    }
}