using Application.Abstractions.Messaging;
using Application.Common.Interfaces;
using Application.Common.Mod.ViewModels;
using Application.CQRS.Goal.Commands;
using Domain.Entities;
using Domain.Shared;
using MapsterMapper;
using Microsoft.Extensions.Logging;

namespace Application.CQRS.Goal.Handlers;

public sealed class CreateOrUpdateUserUsageGoalHandler(
    ILogger<CreateOrUpdateUserUsageGoalHandler> logger,
    IMapper mapper,
    IUserUsageGoalService service)
    : ICommandHandler<CreateOrUpdateUserUsageGoalCommand, UserUsageGoalViewModel>
{
    public async Task<Result<UserUsageGoalViewModel>> Handle(
        CreateOrUpdateUserUsageGoalCommand command,
        CancellationToken cancellationToken)
    {
        var userIdGuid = Guid.Parse(command.UserId);
        var dailyLimitTime = TimeSpan.Parse(command.DailyLimit);
        
        var goal = UserUsageGoal.Create(
            command.Id,
            userIdGuid,
            command.PlatformId,
            dailyLimitTime
        );
        
        var goalResult = await service.CreateOrUpdateUserUsageGoalAsync(goal, cancellationToken);
        
        if (goalResult.IsFailure)
        {
            logger.LogWarning("Failed to create or update goal: {Error}", goalResult.Error);
            return goalResult.Error;
        }

        var viewModel = mapper.Map<UserUsageGoalViewModel>(goalResult.Value);
        return viewModel;
    }
}