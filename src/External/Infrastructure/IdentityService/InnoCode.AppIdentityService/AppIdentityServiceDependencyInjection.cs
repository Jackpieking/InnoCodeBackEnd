using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace InnoCode.AppIdentityService;

public static class AppIdentityServiceDependencyInjection
{
    public static IServiceCollection Config(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        return services;
    }
}
