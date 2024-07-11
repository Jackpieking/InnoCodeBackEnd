using System.Threading;
using System.Threading.Tasks;
using InnoCode.Domain.Entities;

namespace InnoCode.Domain.UnitOfWorks.Main.Repositories.Auth;

public interface IForgotPasswordRepository
{
    Task<UserEntity> FindUserByEmailAsync(string email);

    Task<bool> IsUserFoundByNormalizedEmailQueryAsync(string email, CancellationToken ct);

    Task<UserEntity> GetUserByEmailQueryAsync(string email, CancellationToken ct);

    Task<string> GeneratePasswordResetTokenAsync(UserEntity user);

    Task<bool> AddUserPasswordChangingTokenCommandAsync(
        UserTokenEntity userTokenEntity,
        CancellationToken ct
    );
}
