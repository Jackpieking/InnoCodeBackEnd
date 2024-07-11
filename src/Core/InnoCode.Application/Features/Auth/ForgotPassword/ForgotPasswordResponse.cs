using InnoCode.Application.Share.Features;

namespace InnoCode.Application.Features.Auth.ForgotPassword;

public sealed class ForgotPasswordResponse : IFeatureResponse
{
    public ForgotPasswordResponseStatusCode StatusCode { get; init; }
}
