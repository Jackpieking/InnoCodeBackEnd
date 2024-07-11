using InnoCode.Domain.Entities;
using InnoCode.PostgresSql.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InnoCode.PostgresSql.Data.EntityConfigurations;

internal sealed class RoleClaimEntityConfiguration : IEntityTypeConfiguration<RoleClaimEntity>
{
    public void Configure(EntityTypeBuilder<RoleClaimEntity> builder)
    {
        builder.ToTable(
            name: MetaData.Table.TableName,
            schema: MetaData.Table.TableSchema,
            buildAction: table => table.HasComment(comment: "Contain role claims.")
        );

        builder
            .HasOne(navigationExpression: roleClaim => roleClaim.Role)
            .WithMany(navigationExpression: role => role.RoleClaims)
            .HasForeignKey(foreignKeyExpression: roleClaim => roleClaim.RoleId)
            .OnDelete(deleteBehavior: DeleteBehavior.Cascade);
    }

    internal static class MetaData
    {
        internal static class Table
        {
            internal static readonly string TableName = "role_claim";

            internal static readonly string TableSchema = CommonConstant.DatabaseSchemaName.DEFAULT;
        }
    }
}
