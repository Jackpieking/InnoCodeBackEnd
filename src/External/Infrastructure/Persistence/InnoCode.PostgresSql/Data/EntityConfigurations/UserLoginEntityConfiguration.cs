using InnoCode.Domain.Entities;
using InnoCode.PostgresSql.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InnoCode.PostgresSql.Data.EntityConfigurations;

internal sealed class UserLoginEntityConfiguration : IEntityTypeConfiguration<UserLoginEntity>
{
    public void Configure(EntityTypeBuilder<UserLoginEntity> builder)
    {
        builder.ToTable(
            name: MetaData.Table.TableName,
            schema: MetaData.Table.TableSchema,
            buildAction: table => table.HasComment(comment: "Contain user logins.")
        );

        builder
            .HasOne(navigationExpression: userLogin => userLogin.User)
            .WithMany(navigationExpression: user => user.UserLogins)
            .HasForeignKey(foreignKeyExpression: userLogin => userLogin.UserId)
            .OnDelete(deleteBehavior: DeleteBehavior.Cascade);
    }

    internal static class MetaData
    {
        internal static class Table
        {
            internal static readonly string TableName = "user_login";

            internal static readonly string TableSchema = CommonConstant.DatabaseSchemaName.DEFAULT;
        }
    }
}
