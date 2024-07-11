using System;
using InnoCode.Application.Share.Common;
using InnoCode.Configuration.Infrastructure.Persistence.AspNetCoreIdentity;
using InnoCode.Configuration.Infrastructure.Persistence.Database;
using InnoCode.Domain.Entities;
using InnoCode.Domain.UnitOfWorks.Main;
using InnoCode.PostgresSql.Data;
using InnoCode.PostgresSql.UnitOfWorks.Main;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace InnoCode.PostgresSql;

public static class PostgresSqlDependencyInjection
{
    public static IServiceCollection Config(
        IServiceCollection services,
        IConfiguration configuration
    )
    {
        services.AddDbContextPool<InnoCodeContext>(optionsAction: config =>
        {
            var option = configuration
                .GetRequiredSection(key: "Database")
                .GetRequiredSection(key: "Relational")
                .GetRequiredSection(key: "Main")
                .Get<InnoCodeDatabaseOption>();

            config
                .UseNpgsql(
                    connectionString: option.ConnectionString,
                    npgsqlOptionsAction: npgsqlOptionsAction =>
                    {
                        npgsqlOptionsAction
                            .CommandTimeout(commandTimeout: option.CommandTimeOut)
                            .EnableRetryOnFailure(maxRetryCount: option.EnableRetryOnFailure)
                            .MigrationsAssembly(
                                assemblyName: typeof(InnoCodeContext).Assembly.FullName
                            );
                    }
                )
                .EnableSensitiveDataLogging(
                    sensitiveDataLoggingEnabled: option.EnableSensitiveDataLogging
                )
                .EnableDetailedErrors(detailedErrorsEnabled: option.EnableDetailedErrors)
                .EnableThreadSafetyChecks(enableChecks: option.EnableThreadSafetyChecks)
                .EnableServiceProviderCaching(
                    cacheServiceProvider: option.EnableServiceProviderCaching
                );
        });

        // ====
        services
            .AddIdentity<UserEntity, RoleEntity>(setupAction: config =>
            {
                var option = configuration
                    .GetRequiredSection(key: "AspNetCoreIdentity")
                    .Get<AspNetCoreIdentityOption>();

                config.Password.RequireDigit = option.Password.RequireDigit;
                config.Password.RequireLowercase = option.Password.RequireLowercase;
                config.Password.RequireNonAlphanumeric = option.Password.RequireNonAlphanumeric;
                config.Password.RequireUppercase = option.Password.RequireUppercase;
                config.Password.RequiredLength = option.Password.RequiredLength;
                config.Password.RequiredUniqueChars = option.Password.RequiredUniqueChars;

                config.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromSeconds(
                    value: option.Lockout.DefaultLockoutTimeSpanInSecond
                );
                config.Lockout.MaxFailedAccessAttempts = option.Lockout.MaxFailedAccessAttempts;
                config.Lockout.AllowedForNewUsers = option.Lockout.AllowedForNewUsers;

                config.User.AllowedUserNameCharacters = option.User.AllowedUserNameCharacters;
                config.User.RequireUniqueEmail = option.User.RequireUniqueEmail;

                config.SignIn.RequireConfirmedEmail = option.SignIn.RequireConfirmedEmail;
                config.SignIn.RequireConfirmedPhoneNumber = option
                    .SignIn
                    .RequireConfirmedPhoneNumber;
            })
            .AddEntityFrameworkStores<InnoCodeContext>()
            .AddDefaultTokenProviders();

        #region CustomServices
        services.MakeScopedLazy<InnoCodeContext>();

        services.AddScoped<IMainUnitOfWork, MainUnitOfWork>().MakeScopedLazy<IMainUnitOfWork>();

        // ====
        services.MakeScopedLazy<UserManager<UserEntity>>();

        // ====
        services.MakeScopedLazy<RoleManager<RoleEntity>>();

        // ====
        services.MakeScopedLazy<SignInManager<UserEntity>>();
        #endregion

        return services;
    }
}
