using Infrastructure.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Infrastructure.OptionsSetup;

public class FirebaseOptionsSetup(IConfiguration configuration) : IConfigureOptions<FirebaseOptions>
{
    private const string SectionName = "Firebase";

    public void Configure(FirebaseOptions options)
    {
        configuration.GetSection(SectionName).Bind(options);
    }
}
