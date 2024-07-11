using System;
using System.Threading;
using System.Threading.Tasks;
using InnoCode.Domain.Entities;
using InnoCode.Domain.UnitOfWorks.Main.Repositories.Auth;
using InnoCode.PostgresSql.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace InnoCode.PostgresSql.UnitOfWorks.Main.Repositories.Auth;

internal sealed class RegisterRepository : IRegisterRepository
{
    private readonly Lazy<InnoCodeContext> _context;
    private readonly Lazy<UserManager<UserEntity>> _userManager;

    internal RegisterRepository(
        Lazy<InnoCodeContext> context,
        Lazy<UserManager<UserEntity>> userManager
    )
    {
        _context = context;
        _userManager = userManager;
    }

    public async Task<bool> CreateAndAddUserToRoleCommandAsync(
        UserEntity newUser,
        string password,
        CancellationToken ct
    )
    {
        var dbResult = false;

        await _context
            .Value.Database.CreateExecutionStrategy()
            .ExecuteAsync(operation: async () =>
            {
                await using var dbTransaction = await _context.Value.Database.BeginTransactionAsync(
                    cancellationToken: ct
                );

                try
                {
                    var result = await _userManager.Value.CreateAsync(
                        user: newUser,
                        password: password
                    );

                    if (!result.Succeeded)
                    {
                        throw new DbUpdateConcurrencyException();
                    }

                    result = await _userManager.Value.AddToRoleAsync(user: newUser, role: "USER");

                    if (!result.Succeeded)
                    {
                        throw new DbUpdateConcurrencyException();
                    }

                    await dbTransaction.CommitAsync(cancellationToken: ct);

                    dbResult = true;
                }
                catch
                {
                    await dbTransaction.RollbackAsync(cancellationToken: ct);
                }
            });

        return dbResult;
    }

    public Task<bool> IsUserFoundByNormalizedEmailQueryAsync(string email, CancellationToken ct)
    {
        email = email.ToUpper();

        return _context
            .Value.Set<UserEntity>()
            .AnyAsync(predicate: user => user.NormalizedEmail.Equals(email), cancellationToken: ct);
    }

    public async Task<bool> ValidateUserPasswordAsync(UserEntity newUser, string newPassword)
    {
        IdentityResult result = default;

        foreach (var validator in _userManager.Value.PasswordValidators)
        {
            result = await validator.ValidateAsync(_userManager.Value, newUser, newPassword);
        }

        if (Equals(result, default))
        {
            return false;
        }

        return result.Succeeded;
    }
}
