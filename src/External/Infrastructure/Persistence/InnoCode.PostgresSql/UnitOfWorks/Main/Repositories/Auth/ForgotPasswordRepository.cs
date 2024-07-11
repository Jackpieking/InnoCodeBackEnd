using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using InnoCode.Domain.Entities;
using InnoCode.Domain.UnitOfWorks.Main.Repositories.Auth;
using InnoCode.PostgresSql.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace InnoCode.PostgresSql.UnitOfWorks.Main.Repositories.Auth;

internal sealed class ForgotPasswordRepository : IForgotPasswordRepository
{
    private readonly Lazy<InnoCodeContext> _context;
    private readonly Lazy<UserManager<UserEntity>> _userManager;

    internal ForgotPasswordRepository(
        Lazy<InnoCodeContext> context,
        Lazy<UserManager<UserEntity>> userManager
    )
    {
        _context = context;
        _userManager = userManager;
    }

    public async Task<bool> AddUserPasswordChangingTokenCommandAsync(
        UserTokenEntity userTokenEntity,
        CancellationToken ct
    )
    {
        try
        {
            await _context
                .Value.Set<UserTokenEntity>()
                .AddAsync(entity: userTokenEntity, cancellationToken: ct);

            await _context.Value.SaveChangesAsync(cancellationToken: ct);
        }
        catch
        {
            return false;
        }

        return true;
    }

    public Task<UserEntity> GetUserByEmailQueryAsync(string email, CancellationToken ct)
    {
        email = email.ToUpper();

        return _context
            .Value.Set<UserEntity>()
            .AsNoTracking()
            .Where(predicate: user => user.NormalizedEmail.Equals(email))
            .Select(selector: user => new UserEntity { Id = user.Id })
            .FirstOrDefaultAsync(cancellationToken: ct);
    }

    public Task<bool> IsUserFoundByNormalizedEmailQueryAsync(string email, CancellationToken ct)
    {
        email = email.ToUpper();

        return _context
            .Value.Set<UserEntity>()
            .AnyAsync(predicate: user => user.NormalizedEmail.Equals(email), cancellationToken: ct);
    }

    public Task<UserEntity> FindUserByEmailAsync(string email)
    {
        return _userManager.Value.FindByEmailAsync(email);
    }

    public Task<string> GeneratePasswordResetTokenAsync(UserEntity user)
    {
        return _userManager.Value.GeneratePasswordResetTokenAsync(user);
    }
}
