using System;
using InnoCode.Domain.Entities;
using InnoCode.Domain.UnitOfWorks.Main;
using InnoCode.Domain.UnitOfWorks.Main.Repositories.Auth;
using InnoCode.PostgresSql.Data;
using InnoCode.PostgresSql.UnitOfWorks.Main.Repositories.Auth;
using Microsoft.AspNetCore.Identity;

namespace InnoCode.PostgresSql.UnitOfWorks.Main;

internal sealed class MainUnitOfWork : IMainUnitOfWork
{
    private readonly Lazy<InnoCodeContext> _context;
    private readonly Lazy<UserManager<UserEntity>> _userManager;
    private readonly Lazy<SignInManager<UserEntity>> _signInManager;
    private readonly Lazy<RoleManager<RoleEntity>> _roleManager;

    public MainUnitOfWork(
        Lazy<InnoCodeContext> context,
        Lazy<UserManager<UserEntity>> userManager,
        Lazy<SignInManager<UserEntity>> signInManager,
        Lazy<RoleManager<RoleEntity>> roleManager
    )
    {
        _context = context;
        _userManager = userManager;
        _signInManager = signInManager;
        _roleManager = roleManager;
    }

    public Lazy<ILoginRepository> LoginRepository
    {
        get { return new(() => new LoginRepository(_context, _userManager, _signInManager)); }
    }

    public Lazy<IRegisterRepository> RegisterRepository
    {
        get { return new(() => new RegisterRepository(_context, _userManager)); }
    }

    public Lazy<ILogoutRepository> LogoutRepository
    {
        get { return new(() => new LogoutRepository(_context)); }
    }
}
