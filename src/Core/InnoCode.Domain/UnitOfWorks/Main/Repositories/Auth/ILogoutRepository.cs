using System.Threading;
using System.Threading.Tasks;

namespace InnoCode.Domain.UnitOfWorks.Main.Repositories.Auth;

public interface ILogoutRepository
{
    Task<bool> RemoveRefreshTokenCommandAsync(string refreshTokenId, CancellationToken ct);

    Task<bool> IsRefreshTokenFoundQueryAsync(string refreshTokenId, CancellationToken ct);
}
