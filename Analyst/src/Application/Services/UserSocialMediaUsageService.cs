using Application.Common.Interfaces;
using Domain.Entities;
using Domain.Errors;
using Domain.Interfaces;
using Domain.Shared;
using Domain.Specification.Usages;

namespace Application.Services;

public class UserSocialMediaUsageService(
    IUnitOfWork unitOfWork) : IUserSocialMediaUsageService
{
    public async Task<Result<UserSocialMediaUsage>> AddUserSocialMediaUsageAsync(
        UserSocialMediaUsage userSocialMediaUsage,
        CancellationToken cancellationToken)
    {
        var newUserSocialMediaUsage =
            await unitOfWork.Repository<UserSocialMediaUsage, int>()
                .AddAsync(userSocialMediaUsage,
                    cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return newUserSocialMediaUsage.IsFailure
            ? newUserSocialMediaUsage.Error
            : newUserSocialMediaUsage.Value;
    }


    public async Task<Result<UserSocialMediaUsage>> UpdateUserSocialMediaUsageAsync(
        UserSocialMediaUsage userSocialMediaUsage,
        CancellationToken cancellationToken)
    {
        var newUserSocialMediaUsage = await unitOfWork.Repository<UserSocialMediaUsage, int>()
            .SoftUpdateAsync(userSocialMediaUsage,
                cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return newUserSocialMediaUsage.IsFailure
            ? newUserSocialMediaUsage.Error
            : newUserSocialMediaUsage.Value;
    }


    public async Task<Result<UserSocialMediaUsage>> CreateOrUpdateUserSocialMediaUsageAsync(
        UserSocialMediaUsage userSocialMediaUsage,
        CancellationToken cancellationToken)
    {
        var updateResult = await UpdateUserSocialMediaUsageAsync(
            userSocialMediaUsage,
            cancellationToken);

        if (updateResult.IsSuccess)
            return updateResult;

        if (updateResult.Error == DomainErrors.NotFound<UserSocialMediaUsage>())
            return await AddUserSocialMediaUsageAsync(
                userSocialMediaUsage,
                cancellationToken);

        return updateResult;
    }

    public async Task<Result<List<UserSocialMediaUsage>>> GetByUserIdAsync(
        Guid userId,
        CancellationToken cancellationToken)
    {
        var spec = new UsagesWithPlatformByUserIdSpecification(userId);

        var result = await unitOfWork.Repository<UserSocialMediaUsage, int>()
            .FindManyAsync(spec,
                cancellationToken);

        if (result.IsSuccess)
            return result.Value;
        else
            return result.Error;
    }

}