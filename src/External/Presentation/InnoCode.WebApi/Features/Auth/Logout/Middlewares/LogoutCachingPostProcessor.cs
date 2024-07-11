using System;
using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using InnoCode.Application.Features.Auth.Logout;
using InnoCode.Application.Share.Caching;
using InnoCode.WebApi.Features.Auth.Logout.Common;
using InnoCode.WebApi.Features.Auth.Logout.HttpResponse;
using Microsoft.Extensions.DependencyInjection;

namespace InnoCode.WebApi.Features.Auth.Logout.Middlewares;

internal sealed class LogoutCachingPostProcessor
    : PostProcessor<EmptyRequest, LogoutStateBag, LogoutHttpResponse>
{
    private readonly Lazy<IServiceScopeFactory> _serviceScopeFactory;

    public LogoutCachingPostProcessor(Lazy<IServiceScopeFactory> serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }

    public override async Task PostProcessAsync(
        IPostProcessorContext<EmptyRequest, LogoutHttpResponse> context,
        LogoutStateBag state,
        CancellationToken ct
    )
    {
        if (Equals(objA: context.Response, objB: default))
        {
            return;
        }

        await using var scope = _serviceScopeFactory.Value.CreateAsyncScope();

        var cacheHandler = scope.Resolve<Lazy<ICacheHandler>>();

        // Set new cache if current app code is suitable.
        if (
            context.Response.AppCode.Equals(
                value: LogoutResponseStatusCode.OPERATION_SUCCESS.ToAppCode()
            )
        ) { }
    }
}
