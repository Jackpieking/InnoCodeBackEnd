using InnoCode.Application.Share.Features;

namespace InnoCode.Application.Features.Auth.Register;

public sealed class RegisterResponse : IFeatureResponse
{
    public RegisterResponseStatusCode StatusCode { get; init; }
}
