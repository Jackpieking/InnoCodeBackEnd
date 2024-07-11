using FastEndpoints;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace InnoCode.Application;

public static class ApplicationDependencyInjection
{
    public static IServiceCollection Config(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        // ====
        services.AddFastEndpoints();

        return services;
    }
}
