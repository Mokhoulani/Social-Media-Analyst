using MapsterMapper;
using Application.Abstractions.Messaging;
using Application.Common.Interfaces;
using Application.Common.Mod.ViewModels;
using Application.CQRS.Goal.Queries;
using Domain.Errors;
using Domain.Shared;


namespace Application.CQRS.Goal.Handlers;

public class GetUserUsageGoalsByUserIdHandler(
    IUserUsageGoalService goalService,
    IMapper mapper)
    : IQueryHandler<GetUserUsageGoalsByUserIdQuery, List<UserUsageGoalViewModel>>
{
    public async Task<Result<List<UserUsageGoalViewModel>>> Handle(
        GetUserUsageGoalsByUserIdQuery command,
     CancellationToken cancellationToken)
    {
        if (!Guid.TryParse(command.UserId, out var userId))
        {
            return Result.Failure<List<UserUsageGoalViewModel>>(DomainErrors.UserUsageGoal.NotFound);
        }

        var goalsResult = await goalService.GetByUserIdAsync(userId, cancellationToken);

        if (goalsResult.IsFailure)
        {
            return Result.Failure<List<UserUsageGoalViewModel>>(DomainErrors.UserUsageGoal.NotFound);
        }

        var viewModels = mapper.Map<List<UserUsageGoalViewModel>>(goalsResult.Value);

        return Result.Success(viewModels);
    }

}


