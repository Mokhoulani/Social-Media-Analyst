using Domain.Entities;
using Domain.Shared;

namespace Application.Common.Interfaces;

public interface IUserUsageGoalService
{
    Task<Result<UserUsageGoal>> AddUserUsageGoalAsync(UserUsageGoal userUsageGoal, CancellationToken cancellationToken);
    Task<Result<UserUsageGoal>> UpdateUserUsageGoalAsync(UserUsageGoal userUsageGoal, CancellationToken cancellationToken);
    Task<Result<UserUsageGoal>> CreateOrUpdateUserUsageGoalAsync(UserUsageGoal userUsageGoal,
        CancellationToken cancellationToken);
    Task<Result<List<UserUsageGoal>>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken);
}