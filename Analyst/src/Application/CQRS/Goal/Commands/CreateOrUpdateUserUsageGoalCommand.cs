using Application.Abstractions.Messaging;
using Application.Common.Mod.ViewModels;
using Domain.Shared;

namespace Application.CQRS.Goal.Commands;

public record CreateOrUpdateUserUsageGoalCommand(
    int Id,
    string UserId,
    int PlatformId,
    string DailyLimit)
    : ICommand<Result<UserUsageGoalViewModel>>;