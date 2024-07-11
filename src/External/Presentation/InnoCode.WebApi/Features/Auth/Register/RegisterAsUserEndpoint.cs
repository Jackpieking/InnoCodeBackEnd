using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using InnoCode.Application.Features.Auth.Register;
using InnoCode.WebApi.Features.Auth.Register.HttpResponse;
using InnoCode.WebApi.Features.Auth.Register.Middlewares;
using Microsoft.AspNetCore.Http;

namespace InnoCode.WebApi.Features.Auth.Register;

internal sealed class RegisterEndpoint : Endpoint<RegisterRequest, RegisterHttpResponse>
{
    public override void Configure()
    {
        Post(routePatterns: "auth/register");
        AllowAnonymous();
        DontThrowIfValidationFails();
        PreProcessor<RegisterValidationPreProcessor>();
        Description(builder: builder =>
        {
            builder.ClearDefaultProduces(statusCodes: StatusCodes.Status400BadRequest);
        });
        Summary(endpointSummary: summary =>
        {
            summary.Summary = "Endpoint for user register/signup feature";
            summary.Description = "This endpoint is used for user register/signup purpose.";
            summary.ExampleRequest = new() { Email = "string", Password = "string" };
            summary.Response<RegisterHttpResponse>(
                description: "Represent successful operation response.",
                example: new()
                {
                    AppCode = RegisterResponseStatusCode.OPERATION_SUCCESS.ToAppCode()
                }
            );
        });
    }

    public override async Task<RegisterHttpResponse> ExecuteAsync(
        RegisterRequest req,
        CancellationToken ct
    )
    {
        // Get app feature response.
        var appResponse = await req.ExecuteAsync(ct: ct);

        // Convert to http response.
        var httpResponse = RegisterHttpResponseManager
            .Resolve(statusCode: appResponse.StatusCode)
            .Invoke(arg1: req, arg2: appResponse);

        /*
         * Store the real http code of http response into a temporary variable.
         * Set the http code of http response to default for not serializing.
         */
        var httpCode = httpResponse.HttpCode;
        httpResponse.HttpCode = default;

        // Send http response to client.
        await SendAsync(response: httpResponse, statusCode: httpCode, cancellation: ct);

        // Set the http code of http response back.
        httpResponse.HttpCode = httpCode;

        return httpResponse;
    }
}
