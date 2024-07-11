using System.Threading;
using System.Threading.Tasks;
using InnoCode.Domain.Entities;

namespace InnoCode.Domain.UnitOfWorks.Main.Repositories.Auth;

public interface IRegisterRepository
{
    Task<bool> IsUserFoundByNormalizedEmailQueryAsync(string email, CancellationToken ct);

    Task<bool> CreateAndAddUserToRoleCommandAsync(
        UserEntity newUser,
        string password,
        CancellationToken ct
    );

    Task<bool> ValidateUserPasswordAsync(UserEntity newUser, string newPassword);
}
