using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using InnoCode.Application.Features.Auth.Register;
using InnoCode.WebApi.Features.Auth.Register.Common;
using InnoCode.WebApi.Features.Auth.Register.HttpResponse;

namespace InnoCode.WebApi.Features.Auth.Register.Middlewares;

internal sealed class RegisterValidationPreProcessor
    : PreProcessor<RegisterRequest, RegisterStateBag>
{
    public override async Task PreProcessAsync(
        IPreProcessorContext<RegisterRequest> context,
        RegisterStateBag state,
        CancellationToken ct
    )
    {
        if (context.HasValidationFailures)
        {
            var httpResponse = RegisterHttpResponseManager
                .Resolve(statusCode: RegisterResponseStatusCode.INPUT_VALIDATION_FAIL)
                .Invoke(
                    arg1: context.Request,
                    arg2: new() { StatusCode = RegisterResponseStatusCode.INPUT_VALIDATION_FAIL }
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
