using Infrastructure.Authentication;
using Microsoft.Extensions.Options;

namespace Api.OptionsSetup;

public class RefreshTokenOptionsSetup(IConfiguration configuration) : IConfigureOptions<RefreshTokenOptions>
{
    private const string SectionName = "RefreshToken";

    public void Configure(RefreshTokenOptions options)
    {
        configuration.GetSection(SectionName).Bind(options);
    }
}
