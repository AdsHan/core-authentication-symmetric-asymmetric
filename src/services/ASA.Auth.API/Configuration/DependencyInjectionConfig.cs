using ASA.Auth.API.Service;
using ASA.Core.Authentication;

namespace ASA.Auth.API.Configuration;

public static class DependencyInjectionConfig
{
    public static IServiceCollection AddDependencyConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IGenerateSecurityKey, GenerateSecurityKey>();

        services.AddScoped<AuthService>();

        return services;
    }
}
