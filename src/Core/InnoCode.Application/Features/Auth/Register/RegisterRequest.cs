using InnoCode.Application.Share.Features;

namespace InnoCode.Application.Features.Auth.Register;

public sealed class RegisterRequest : IFeatureRequest<RegisterResponse>
{
    public string Email { get; init; }

    public string Password { get; init; }
}
