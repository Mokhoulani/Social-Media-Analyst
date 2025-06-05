using Application.Abstractions.Messaging;
using Application.Common.Mod.ViewModels;
using Domain.Shared;

namespace Application.CQRS.Usage.Commands;

public record CreateOrUpdateUserSocialMediaUsageCommand(
    int Id,
    string UserId,
    int PlatformId,
    string StartTime)
    : ICommand<Result<UserSocialMediaUsageViewModel>>;