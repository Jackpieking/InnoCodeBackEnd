using InnoCode.Application.Share.Features;

namespace InnoCode.Application.Features.Auth.ForgotPassword;

public sealed class ForgotPasswordRequest : IFeatureRequest<ForgotPasswordResponse>
{
    public string Email { get; set; }
}
