using InnoCode.Application.Share.Features;

namespace InnoCode.Application.Features.Auth.Login;

public sealed class LoginRequest : IFeatureRequest<LoginResponse>
{
    public string Email { get; init; }

    public string Password { get; init; }

    public bool IsRememberMe { get; init; }
}
