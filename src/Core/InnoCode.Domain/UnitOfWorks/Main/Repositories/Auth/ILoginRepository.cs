using System.Threading;
using System.Threading.Tasks;
using InnoCode.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace InnoCode.Domain.UnitOfWorks.Main.Repositories.Auth;

public interface ILoginRepository
{
    Task<UserEntity> FindUserByEmailAsync(string email, CancellationToken ct);

    Task<SignInResult> CheckPasswordSignInAsync(
        UserEntity user,
        string password,
        bool lockOutOnFailure,
        CancellationToken ct
    );

    Task<bool> IsUserInRoleAsync(UserEntity user, string role, CancellationToken ct);

    Task<bool> CreateRefreshTokenCommandAsync(UserTokenEntity refreshToken, CancellationToken ct);
}
