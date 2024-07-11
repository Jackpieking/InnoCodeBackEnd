using InnoCode.Application.Share.Features;

namespace InnoCode.Application.Features.Auth.Logout;

public sealed class LogoutRequest : IFeatureRequest<LogoutResponse>
{
    private string _refreshTokenId;

    public string GetRefreshTokenId() => _refreshTokenId;

    public void SetRefreshTokenId(string refreshTokenId) => _refreshTokenId = refreshTokenId;
}
