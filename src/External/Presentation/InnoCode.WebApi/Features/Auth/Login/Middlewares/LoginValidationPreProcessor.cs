using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using InnoCode.Application.Features.Auth.Login;
using InnoCode.WebApi.Features.Auth.Login.Common;
using InnoCode.WebApi.Features.Auth.Login.HttpResponse;

namespace InnoCode.WebApi.Features.Auth.Login.Middlewares;

internal sealed class LoginValidationPreProcessor : PreProcessor<LoginRequest, LoginStateBag>
{
    public override async Task PreProcessAsync(
        IPreProcessorContext<LoginRequest> context,
        LoginStateBag state,
        CancellationToken ct
    )
    {
        if (context.HasValidationFailures)
        {
            var httpResponse = LoginHttpResponseManager
                .Resolve(statusCode: LoginResponseStatusCode.INPUT_VALIDATION_FAIL)
                .Invoke(
                    arg1: context.Request,
                    arg2: new() { StatusCode = LoginResponseStatusCode.INPUT_VALIDATION_FAIL }
                );

            // Save http code temporarily and set http code of response to default for not serializing.
            var httpCode = httpResponse.HttpCode;
            httpResponse.HttpCode = default;

            await context.HttpContext.Response.SendAsync(
                response: httpResponse,
                statusCode: httpCode,
                cancellation: ct
            );

            // Restore http code of response.
            httpResponse.HttpCode = httpCode;

            return;
        }
    }
}
