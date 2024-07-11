using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using InnoCode.Application.Features.Auth.Login;
using InnoCode.WebApi.Features.Auth.Login.HttpResponse;
using InnoCode.WebApi.Features.Auth.Login.Middlewares;
using Microsoft.AspNetCore.Http;

namespace InnoCode.WebApi.Features.Auth.Login;

internal sealed class LoginEndpoint : Endpoint<LoginRequest, LoginHttpResponse>
{
    public override void Configure()
    {
        Post(routePatterns: "auth/login");
        AllowAnonymous();
        DontThrowIfValidationFails();
        PreProcessor<LoginValidationPreProcessor>();
        Description(builder: builder =>
        {
            builder.ClearDefaultProduces(statusCodes: StatusCodes.Status400BadRequest);
        });
        Summary(endpointSummary: summary =>
        {
            summary.Summary = "Endpoint for user login feature";
            summary.Description = "This endpoint is used for user login purpose.";
            summary.ExampleRequest = new()
            {
                Email = "string",
                Password = "string",
                IsRememberMe = true
            };
            summary.Response<LoginHttpResponse>(
                description: "Represent successful operation response.",
                example: new() { AppCode = LoginResponseStatusCode.OPERATION_SUCCESS.ToAppCode() }
            );
        });
    }

    public override async Task<LoginHttpResponse> ExecuteAsync(
        LoginRequest req,
        CancellationToken ct
    )
    {
        // Get app feature response.
        var appResponse = await req.ExecuteAsync(ct: ct);

        // Convert to http response.
        var httpResponse = LoginHttpResponseManager
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
