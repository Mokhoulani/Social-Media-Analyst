using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Infrastructure.Authentication;

public class RefreshTokenOptionsSetup(IConfiguration configuration) : IConfigureOptions<RefreshTokenOptions>
{
    private const string SectionName = "RefreshToken";

    public void Configure(RefreshTokenOptions options)
    {
        configuration.GetSection(SectionName).Bind(options);
    }
}