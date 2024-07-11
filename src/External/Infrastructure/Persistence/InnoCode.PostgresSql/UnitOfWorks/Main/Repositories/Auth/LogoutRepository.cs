using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using InnoCode.Domain.Entities;
using InnoCode.Domain.UnitOfWorks.Main.Repositories.Auth;
using InnoCode.PostgresSql.Data;
using Microsoft.EntityFrameworkCore;

namespace InnoCode.PostgresSql.UnitOfWorks.Main.Repositories.Auth;

internal sealed class LogoutRepository : ILogoutRepository
{
    private readonly Lazy<InnoCodeContext> _context;

    internal LogoutRepository(Lazy<InnoCodeContext> context)
    {
        _context = context;
    }

    public Task<bool> IsRefreshTokenFoundQueryAsync(string refreshTokenId, CancellationToken ct)
    {
        return _context
            .Value.Set<UserTokenEntity>()
            .AnyAsync(
                predicate: token =>
                    token.LoginProvider.Equals(refreshTokenId) && token.ExpiredAt > DateTime.UtcNow,
                cancellationToken: ct
            );
    }

    public async Task<bool> RemoveRefreshTokenCommandAsync(
        string refreshTokenId,
        CancellationToken ct
    )
    {
        var executedTransactionResult = false;

        await _context
            .Value.Database.CreateExecutionStrategy()
            .ExecuteAsync(operation: async () =>
            {
                await using var dbTransaction = await _context.Value.Database.BeginTransactionAsync(
                    cancellationToken: ct
                );

                try
                {
                    await _context
                        .Value.Set<UserTokenEntity>()
                        .Where(predicate: token => token.LoginProvider.Equals(refreshTokenId))
                        .ExecuteDeleteAsync(cancellationToken: ct);

                    await dbTransaction.CommitAsync(cancellationToken: ct);

                    executedTransactionResult = true;
                }
                catch
                {
                    await dbTransaction.RollbackAsync(cancellationToken: ct);
                }
            });

        return executedTransactionResult;
    }
}
