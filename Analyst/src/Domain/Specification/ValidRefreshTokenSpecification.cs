using Domain.Entities;

namespace Domain.Specification;

public class ValidRefreshTokenSpecification(string refreshToken)
    : Specification<RefreshToken, Guid>(r => r.Token == refreshToken && DateTime.UtcNow < r.ExpiresAt);
