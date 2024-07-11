using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using InnoCode.Application.Features.Auth.Logout;
using InnoCode.WebApi.Features.Auth.Logout.Common;
using InnoCode.WebApi.Features.Auth.Logout.HttpResponse;
using InnoCode.WebApi.Features.Auth.Logout.Middlewares;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;

namespace InnoCode.WebApi.Features.Auth.Logout;

internal sealed class LogoutEndpoint : Endpoint<EmptyRequest, LogoutHttpResponse>
{
    public override void Configure()
    {
        Post(routePatterns: "auth/logout");
        DontThrowIfValidationFails();
        AuthSchemes(authSchemeNames: JwtBearerDefaults.AuthenticationScheme);
        PreProcessor<LogoutValidationPreProcessor>();
        PreProcessor<LogoutAuthorizationPreProcessor>();
        //PostProcessor<LogoutCachingPostProcessor>();
        Description(builder: builder =>
        {
            builder.ClearDefaultProduces(
                StatusCodes.Status400BadRequest,
                StatusCodes.Status401Unauthorized,
                StatusCodes.Status403Forbidden
            );
        });
        Summary(endpointSummary: summary =>
        {
            summary.Summary = "Endpoint for user logout feature";
            summary.Description = "This endpoint is used for user logout purpose.";
            summary.ExampleRequest = new();
            summary.Response<LogoutHttpResponse>(
                description: "Represent successful operation response.",
                example: new() { AppCode = LogoutResponseStatusCode.OPERATION_SUCCESS.ToAppCode() }
            );
        });
    }

    public override async Task<LogoutHttpResponse> ExecuteAsync(
        EmptyRequest req,
        CancellationToken ct
    )
    {
        var stateBag = ProcessorState<LogoutStateBag>();

        // Get app feature response.
        var appResponse = await stateBag.AppRequest.ExecuteAsync(ct: ct);

        // Convert to http response.
        var httpResponse = LogoutHttpResponseManager
            .Resolve(statusCode: appResponse.StatusCode)
            .Invoke(arg1: stateBag.AppRequest, arg2: appResponse);

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
