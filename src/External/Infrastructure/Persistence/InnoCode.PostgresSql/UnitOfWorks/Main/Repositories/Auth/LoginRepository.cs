using System;
using System.Threading;
using System.Threading.Tasks;
using InnoCode.Domain.Entities;
using InnoCode.Domain.UnitOfWorks.Main.Repositories.Auth;
using InnoCode.PostgresSql.Data;
using Microsoft.AspNetCore.Identity;

namespace InnoCode.PostgresSql.UnitOfWorks.Main.Repositories.Auth;

internal sealed class LoginRepository : ILoginRepository
{
    private readonly Lazy<InnoCodeContext> _context;
    private readonly Lazy<UserManager<UserEntity>> _userManager;
    private readonly Lazy<SignInManager<UserEntity>> _signInManager;

    internal LoginRepository(
        Lazy<InnoCodeContext> context,
        Lazy<UserManager<UserEntity>> userManager,
        Lazy<SignInManager<UserEntity>> signInManager
    )
    {
        _context = context;
        _userManager = userManager;
        _signInManager = signInManager;
    }

    public Task<SignInResult> CheckPasswordSignInAsync(
        UserEntity user,
        string password,
        bool lockOutOnFailure,
        CancellationToken ct
    )
    {
        return _signInManager.Value.CheckPasswordSignInAsync(user, password, lockOutOnFailure);
    }

    public async Task<bool> CreateRefreshTokenCommandAsync(
        UserTokenEntity refreshToken,
        CancellationToken ct
    )
    {
        try
        {
            await _context.Value.Set<UserTokenEntity>().AddAsync(refreshToken, ct);

            await _context.Value.SaveChangesAsync(ct);
        }
        catch
        {
            return false;
        }

        return true;
    }

    public Task<UserEntity> FindUserByEmailAsync(string email, CancellationToken ct)
    {
        return _userManager.Value.FindByEmailAsync(email);
    }

    public Task<bool> IsUserInRoleAsync(UserEntity user, string role, CancellationToken ct)
    {
        return _userManager.Value.IsInRoleAsync(user, role);
    }
}
