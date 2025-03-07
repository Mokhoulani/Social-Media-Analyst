using Infrastructure.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Infrastructure.OptionsSetup;

public class SqlOptionsSetup(IConfiguration configuration) : IConfigureOptions<SqlOptions>
{
    private const string SectionName = "DatabaseSettings";

    public void Configure(SqlOptions options)
    {
        configuration.GetSection(SectionName).Bind(options);
    }
}
