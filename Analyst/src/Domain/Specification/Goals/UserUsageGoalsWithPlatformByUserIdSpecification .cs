using Domain.Entities;


namespace Domain.Specification.Goals;

public class UserUsageGoalsWithPlatformByUserIdSpecification : Specification<UserUsageGoal, int>
{
    public UserUsageGoalsWithPlatformByUserIdSpecification(Guid userId)
        : base(goal => goal.UserId == userId)
    {
        AddInclude(goal => goal.Platform);

        AddOrderByDescending(goal => goal.CreatedOnUtc);
    }
}
