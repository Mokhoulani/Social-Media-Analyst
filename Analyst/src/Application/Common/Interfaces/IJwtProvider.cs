using Domain.Entities;

namespace Application.Common.Interfaces;

public interface IJwtProvider
{
   Task<string> GenerateAsync(User user);
}
