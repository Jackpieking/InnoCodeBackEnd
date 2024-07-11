using InnoCode.AppIdentityService.Handlers;
using InnoCode.Application.Share.Common;
using InnoCode.Application.Share.Tokens;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ODour.AppIdentityService.Handlers;

namespace InnoCode.AppIdentityService;

public static class AppIdentityServiceDependencyInjection
{
    public static IServiceCollection Config(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        #region CustomServices
        services
            .AddSingleton<IAccessTokenHandler, AppAccessTokenHandler>()
            .MakeSingletonLazy<IAccessTokenHandler>();

        // ====
        services
            .AddSingleton<IRefreshTokenHandler, AppRefreshTokenHandler>()
            .MakeSingletonLazy<IRefreshTokenHandler>();
        #endregion

        return services;
    }
}
