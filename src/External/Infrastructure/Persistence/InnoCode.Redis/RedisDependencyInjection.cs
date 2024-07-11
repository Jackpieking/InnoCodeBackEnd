using InnoCode.Configuration.Infrastructure.Persistence.Cache.Redis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace InnoCode.Redis;

public static class RedisDependencyInjection
{
    public static IServiceCollection Config(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        // ====
        var option = configuration
            .GetRequiredSection("Database")
            .GetRequiredSection("Cache")
            .GetRequiredSection("Redis")
            .Get<RedisOption>();

        services.AddStackExchangeRedisCache(config =>
        {
            config.Configuration = option.ConnectionString;
        });

        return services;
    }
}
