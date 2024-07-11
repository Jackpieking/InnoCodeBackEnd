using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using InnoCode.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace InnoCode.PostgresSql.Data;

public static class EntityDataSeeding
{
    private static readonly string USER_ROLE = "USER";

    private static readonly string ADMIN_ROLE = "ADMIN";

    private static readonly string ADMIN_EMAIL = "khoaprovn041@gmail.com";

    private static readonly string USER_EMAIL = "khoamapdit03@gmail.com";

    public static async Task<bool> SeedAsync(
        Lazy<InnoCodeContext> context,
        Lazy<UserManager<UserEntity>> userManager,
        Lazy<RoleManager<RoleEntity>> roleManager,
        CancellationToken cancellationToken
    )
    {
        // Check if all tables of database are empty.
        var areTablesEmtpy = await AreAllTablesEmptyAsync(context, cancellationToken);

        if (!areTablesEmtpy)
        {
            return true;
        }

        // Start seeding.
        await MarkAsAlreadySeedAsync(context.Value, cancellationToken);

        var seedRoles = GetSeedRoleEntities();

        var seedUsers = GetSeedUserEntities();

        var executedTransactionResult = false;

        // Altering database.
        await context
            .Value.Database.CreateExecutionStrategy()
            .ExecuteAsync(async () =>
            {
                await using var dbTransaction = await context.Value.Database.BeginTransactionAsync(
                    cancellationToken
                );

                try
                {
                    foreach (var newRole in seedRoles)
                    {
                        await roleManager.Value.CreateAsync(newRole);
                    }

                    // user.
                    var user = seedUsers.Find(user =>
                        user.UserName.Equals(USER_EMAIL, StringComparison.OrdinalIgnoreCase)
                    );

                    await userManager.Value.CreateAsync(user, "Khoa1234");

                    await userManager.Value.AddToRoleAsync(user, role: USER_ROLE);

                    var emailConfirmationToken =
                        await userManager.Value.GenerateEmailConfirmationTokenAsync(user);

                    await userManager.Value.ConfirmEmailAsync(user, emailConfirmationToken);

                    // admin.
                    user = seedUsers.Find(user =>
                        user.UserName.Equals(ADMIN_EMAIL, StringComparison.OrdinalIgnoreCase)
                    );

                    await userManager.Value.CreateAsync(user, "Khoa1234");

                    await userManager.Value.AddToRoleAsync(user, ADMIN_ROLE);

                    emailConfirmationToken =
                        await userManager.Value.GenerateEmailConfirmationTokenAsync(user);

                    await userManager.Value.ConfirmEmailAsync(user, emailConfirmationToken);

                    await dbTransaction.CommitAsync(cancellationToken);

                    executedTransactionResult = true;
                }
                catch
                {
                    await dbTransaction.RollbackAsync(cancellationToken);
                }
            });

        return executedTransactionResult;
    }

    private static async Task<bool> AreAllTablesEmptyAsync(
        Lazy<InnoCodeContext> context,
        CancellationToken cancellationToken
    )
    {
        var areTablesNotEmtpy = await context
            .Value.Set<SeedFlagEntity>()
            .AnyAsync(cancellationToken: cancellationToken);

        if (areTablesNotEmtpy)
        {
            return false;
        }

        // Start seeding.
        await MarkAsAlreadySeedAsync(context: context.Value, cancellationToken: cancellationToken);

        return true;
    }

    private static async Task MarkAsAlreadySeedAsync(
        InnoCodeContext context,
        CancellationToken cancellationToken
    )
    {
        await context.AddAsync(
            entity: new SeedFlagEntity { Id = Guid.NewGuid() },
            cancellationToken: cancellationToken
        );
    }

    private static List<RoleEntity> GetSeedRoleEntities()
    {
        return new()
        {
            new()
            {
                Id = Guid.Parse(input: "c95f4aae-2a41-4c76-9cc4-f1d632409525"),
                Name = ADMIN_ROLE
            },
            new()
            {
                Id = Guid.Parse(input: "8ebe29bc-4706-4fda-bb28-ed127d150c05"),
                Name = USER_ROLE
            }
        };
    }

    private static List<UserEntity> GetSeedUserEntities()
    {
        return new()
        {
            new()
            {
                Id = Guid.Parse(input: "8ebe29bc-4706-4fda-bb28-ed127d150c05"),
                UserName = ADMIN_EMAIL,
                Email = ADMIN_EMAIL,
                PhoneNumber = "0706042250",
            },
            new()
            {
                Id = Guid.Parse(input: "9a2b6683-a16e-4270-a23c-3e09a6e27345"),
                UserName = USER_EMAIL,
                Email = USER_EMAIL,
                PhoneNumber = "0706042250",
            }
        };
    }
}
