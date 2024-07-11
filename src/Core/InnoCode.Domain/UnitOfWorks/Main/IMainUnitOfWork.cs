using System;
using InnoCode.Domain.UnitOfWorks.Main.Repositories.Auth;

namespace InnoCode.Domain.UnitOfWorks.Main;

public interface IMainUnitOfWork
{
    Lazy<ILoginRepository> LoginRepository { get; }

    Lazy<IRegisterRepository> RegisterRepository { get; }

    Lazy<ILogoutRepository> LogoutRepository { get; }
}
