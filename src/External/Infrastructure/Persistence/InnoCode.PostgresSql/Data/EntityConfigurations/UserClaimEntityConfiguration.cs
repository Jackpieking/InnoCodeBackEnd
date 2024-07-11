using InnoCode.Domain.Entities;
using InnoCode.PostgresSql.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InnoCode.PostgresSql.Data.EntityConfigurations;

internal sealed class UserClaimEntityConfiguration : IEntityTypeConfiguration<UserClaimEntity>
{
    public void Configure(EntityTypeBuilder<UserClaimEntity> builder)
    {
        builder.ToTable(
            name: MetaData.Table.TableName,
            schema: MetaData.Table.TableSchema,
            buildAction: table => table.HasComment(comment: "Contain user claims.")
        );

        builder
            .HasOne(navigationExpression: userClaim => userClaim.User)
            .WithMany(navigationExpression: user => user.UserClaims)
            .HasForeignKey(foreignKeyExpression: userClaim => userClaim.UserId)
            .OnDelete(deleteBehavior: DeleteBehavior.Cascade);
    }

    internal static class MetaData
    {
        internal static class Table
        {
            internal static readonly string TableName = "user_claim";

            internal static readonly string TableSchema = CommonConstant.DatabaseSchemaName.DEFAULT;
        }
    }
}
