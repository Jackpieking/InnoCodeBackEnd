using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using InnoCode.Application.Features.Auth.Logout;
using InnoCode.WebApi.Features.Auth.Logout.Common;
using InnoCode.WebApi.Features.Auth.Logout.HttpResponse;

namespace InnoCode.WebApi.Features.Auth.Logout.Middlewares;

internal sealed class LogoutValidationPreProcessor : PreProcessor<EmptyRequest, LogoutStateBag>
{
    public override async Task PreProcessAsync(
        IPreProcessorContext<EmptyRequest> context,
        LogoutStateBag state,
        CancellationToken ct
    )
    {
        if (context.HasValidationFailures)
        {
            var httpResponse = LogoutHttpResponseManager
                .Resolve(statusCode: LogoutResponseStatusCode.INPUT_VALIDATION_FAIL)
                .Invoke(
                    arg1: state.AppRequest,
                    arg2: new() { StatusCode = LogoutResponseStatusCode.INPUT_VALIDATION_FAIL }
                );

            // Save http code temporarily and set http code of response to default for not serializing.
            var httpCode = httpResponse.HttpCode;
            httpResponse.HttpCode = default;

            await context.HttpContext.Response.SendAsync(
                response: httpResponse,
                statusCode: httpCode,
                cancellation: ct
            );

            return;
        }
    }
}
