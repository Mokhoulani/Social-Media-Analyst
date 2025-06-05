using Domain.Entities;


namespace Domain.Specification.Goals;

public class GoalsWithPlatformByUserIdSpecification : Specification<UserUsageGoal, int>
{
    public GoalsWithPlatformByUserIdSpecification(Guid userId)
        : base(goal => goal.UserId == userId)
    {
        AddInclude(goal => goal.Platform);

        AddOrderByDescending(goal => goal.CreatedOnUtc);
    }
}