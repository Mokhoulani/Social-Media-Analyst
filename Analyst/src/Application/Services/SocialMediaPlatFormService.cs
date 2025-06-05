using Application.Common.Interfaces;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Shared;

namespace Application.Services
{
    public sealed class SocialMediaPlatFormService(IUnitOfWork unitOfWork) : ISocialMediaPlatFormService
    {
        public async Task<Result<List<SocialMediaPlatform>>> GetAllPlatforms(CancellationToken cancellationToken)
        {
            var platformsResult = await unitOfWork.Repository<SocialMediaPlatform, int>()
            .GetAllAsync(cancellationToken);

            if (platformsResult.IsSuccess)
                return platformsResult.Value;

            return platformsResult.Error;
        }
    }
}