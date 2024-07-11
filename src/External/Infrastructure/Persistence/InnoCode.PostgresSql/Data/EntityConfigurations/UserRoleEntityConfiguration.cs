using InnoCode.Domain.Entities;
using InnoCode.PostgresSql.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InnoCode.PostgresSql.Data.EntityConfigurations;

internal sealed class UserRoleEntityConfiguration : IEntityTypeConfiguration<UserRoleEntity>
{
    public void Configure(EntityTypeBuilder<UserRoleEntity> builder)
    {
        builder.ToTable(
            name: MetaData.Table.TableName,
            schema: MetaData.Table.TableSchema,
            buildAction: table => table.HasComment(comment: "Contain user roles.")
        );

        builder
            .HasOne(navigationExpression: userRole => userRole.User)
            .WithMany(navigationExpression: user => user.UserRoles)
            .HasForeignKey(foreignKeyExpression: userRole => userRole.UserId)
            .OnDelete(deleteBehavior: DeleteBehavior.Cascade);

        builder
            .HasOne(navigationExpression: userRole => userRole.Role)
            .WithMany(navigationExpression: role => role.UserRoles)
            .HasForeignKey(foreignKeyExpression: userRole => userRole.RoleId)
            .OnDelete(deleteBehavior: DeleteBehavior.Cascade);
    }

    internal static class MetaData
    {
        internal static class Table
        {
            internal static readonly string TableName = "user_role";

            internal static readonly string TableSchema = CommonConstant.DatabaseSchemaName.DEFAULT;
        }
    }
}
