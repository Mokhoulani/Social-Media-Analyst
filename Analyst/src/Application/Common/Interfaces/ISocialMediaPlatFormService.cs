using Domain.Entities;
using Domain.Shared;

namespace Application.Common.Interfaces
{
    public interface ISocialMediaPlatFormService
    {
        Task<Result<List<SocialMediaPlatform>>> GetAllPlatforms(CancellationToken cancellationToken);
    }
}