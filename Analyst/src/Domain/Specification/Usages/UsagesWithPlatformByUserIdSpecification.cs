using Domain.Entities;


namespace Domain.Specification.Usages;

public class UsagesWithPlatformByUserIdSpecification :
    Specification<UserSocialMediaUsage, int>
{
    public UsagesWithPlatformByUserIdSpecification(Guid userId)
        : base(usage => usage.UserId == userId)
    {
        AddInclude(usage => usage.Platform);

        AddOrderByDescending(usage => usage.StartTime);
    }
}