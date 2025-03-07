using Infrastructure.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Infrastructure.OptionsSetup;


public class RedisOptionsSetup(IConfiguration configuration) : IConfigureOptions<RedisOptions>
{
    private const string SectionName = "Redis";

    public void Configure(RedisOptions options)
    {
        configuration.GetSection(SectionName).Bind(options);
    }
}
