using Domain.Entities;
using Domain.Shared;

namespace Application.Common.Interfaces;

public interface IUserSocialMediaUsageService
{
    Task<Result<UserSocialMediaUsage>> AddUserSocialMediaUsageAsync(UserSocialMediaUsage UserSocialMediaUsage,
        CancellationToken cancellationToken);

    Task<Result<UserSocialMediaUsage>> UpdateUserSocialMediaUsageAsync(UserSocialMediaUsage UserSocialMediaUsage,
        CancellationToken cancellationToken);

    Task<Result<UserSocialMediaUsage>> CreateOrUpdateUserSocialMediaUsageAsync(
        UserSocialMediaUsage userSocialMediaUsage,
        CancellationToken cancellationToken);

    Task<Result<List<UserSocialMediaUsage>>> GetByUserIdAsync(Guid userId,
        CancellationToken cancellationToken);
}