using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ASA.Core.Settings;

public static class SettingsConfig
{
    public static IServiceCollection AddSettingsConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        var tokenSettings = configuration.GetSection("TokenSettings").Get<TokenSettings>();
        services.AddSingleton(tokenSettings);

        return services;
    }
}