using InnoCode.Application.Share.Features;

namespace InnoCode.Application.Features.Auth.Logout;

public sealed class LogoutResponse : IFeatureResponse
{
    public LogoutResponseStatusCode StatusCode { get; init; }
}
