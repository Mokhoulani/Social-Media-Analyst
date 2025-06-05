using Application.Common.Interfaces;
using Domain.Entities;
using Domain.Errors;
using Domain.Interfaces;
using Domain.Shared;
using Domain.Specification.Goals;

namespace Application.Services;

public class UserUsageGoalService(
    IUnitOfWork unitOfWork) : IUserUsageGoalService
{
    public async Task<Result<UserUsageGoal>> AddUserUsageGoalAsync(UserUsageGoal userUsageGoal,
        CancellationToken cancellationToken)
    {
        var newUserUsageGoal =
            await unitOfWork.Repository<UserUsageGoal, int>()
                .AddAsync(userUsageGoal,
                    cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return newUserUsageGoal.IsFailure
            ? newUserUsageGoal.Error
            : newUserUsageGoal.Value;
    }


    public async Task<Result<UserUsageGoal>> UpdateUserUsageGoalAsync(
        UserUsageGoal userUsageGoal,
        CancellationToken cancellationToken)
    {
        var newUserUsageGoal = await unitOfWork.Repository<UserUsageGoal, int>()
            .SoftUpdateAsync(userUsageGoal,
                cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return newUserUsageGoal.IsFailure
            ? newUserUsageGoal.Error
            : newUserUsageGoal.Value;
    }


    public async Task<Result<UserUsageGoal>> CreateOrUpdateUserUsageGoalAsync(
        UserUsageGoal userUsageGoal,
        CancellationToken cancellationToken)
    {
        var updateResult = await UpdateUserUsageGoalAsync(userUsageGoal,
            cancellationToken);

        if (updateResult.IsSuccess)
            return updateResult;

        if (updateResult.Error == DomainErrors.NotFound<UserUsageGoal>())
            return await AddUserUsageGoalAsync(userUsageGoal,
                cancellationToken);

        return updateResult;
    }

    public async Task<Result<List<UserUsageGoal>>> GetByUserIdAsync(
        Guid userId,
        CancellationToken cancellationToken)
    {
        var spec = new GoalsWithPlatformByUserIdSpecification(userId);

        var result = await unitOfWork.Repository<UserUsageGoal, int>()
            .FindManyAsync(spec,
                cancellationToken);

        if (result.IsSuccess)
            return result.Value;
        else
            return result.Error;
    }

}