using InnoCode.Application.Features.Auth.Logout;

namespace InnoCode.WebApi.Features.Auth.Logout.Common;

internal sealed class LogoutStateBag
{
    internal LogoutRequest AppRequest { get; } = new();
}
