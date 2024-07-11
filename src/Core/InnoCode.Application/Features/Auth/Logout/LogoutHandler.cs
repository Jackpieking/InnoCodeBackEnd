using System;
using System.Threading;
using System.Threading.Tasks;
using InnoCode.Application.Share.Features;
using InnoCode.Domain.UnitOfWorks.Main;
using InnoCode.Domain.UnitOfWorks.Main.Repositories.Auth;

namespace InnoCode.Application.Features.Auth.Logout;

internal sealed class LogoutHandler : IFeatureHandler<LogoutRequest, LogoutResponse>
{
    private readonly Lazy<ILogoutRepository> _logoutRepository;

    public LogoutHandler(Lazy<IMainUnitOfWork> unitOfWork)
    {
        _logoutRepository = unitOfWork.Value.LogoutRepository;
    }

    public async Task<LogoutResponse> ExecuteAsync(LogoutRequest command, CancellationToken ct)
    {
        // Attempt to remove refresh token by its value.
        var dbResult = await _logoutRepository.Value.RemoveRefreshTokenCommandAsync(
            refreshTokenId: command.GetRefreshTokenId(),
            ct: ct
        );

        if (!dbResult)
        {
            return new() { StatusCode = LogoutResponseStatusCode.OPERATION_FAIL };
        }

        return new() { StatusCode = LogoutResponseStatusCode.OPERATION_SUCCESS };
    }
}
