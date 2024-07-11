using System;
using InnoCode.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace InnoCode.PostgresSql.Data;

public sealed class InnoCodeContext : IdentityDbContext<UserEntity, RoleEntity, Guid>
{
    public InnoCodeContext(DbContextOptions<InnoCodeContext> options)
        : base(options: options) { }

    /// <summary>
    ///     Configure tables and seed initial data here.
    /// </summary>
    /// <param name="builder">
    ///     Model builder access the database.
    /// </param>
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        RemoveAspNetPrefixInIdentityTable(builder);

        InitCaseInsensitiveCollation(builder);

        builder.ApplyConfigurationsFromAssembly(typeof(InnoCodeContext).Assembly);
    }

    /// <summary>
    ///     Remove "AspNet" prefix in identity table name.
    /// </summary>
    /// <param name="builder">
    ///     Model builder access the database.
    /// </param>
    private static void RemoveAspNetPrefixInIdentityTable(ModelBuilder builder)
    {
        const string AspNetPrefix = "AspNet";

        foreach (var entityType in builder.Model.GetEntityTypes())
        {
            var tableName = entityType.GetTableName();

            if (tableName.StartsWith(AspNetPrefix))
            {
                entityType.SetTableName(tableName[6..]);
            }
        }
    }

    /// <summary>
    ///     Create new case insensitive collation.
    /// </summary>
    /// <param name="builder">
    ///     Model builder access the database.
    /// </param>
    private static void InitCaseInsensitiveCollation(ModelBuilder builder)
    {
        builder.HasCollation("case_insensitive", "en-u-ks-primary", "icu", false);
    }
}
