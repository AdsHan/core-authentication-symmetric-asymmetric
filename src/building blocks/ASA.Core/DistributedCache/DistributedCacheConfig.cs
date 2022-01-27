using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ASA.Core.DistributedCache;

public static class DistributedCacheConfig
{
    public static IServiceCollection AddDistributedCacheConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddStackExchangeRedisCache(options =>
        {
            options.InstanceName = "CacheStorage";
            options.Configuration = configuration.GetConnectionString("RedisCs");
        });

        return services;
    }
}
