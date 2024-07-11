using InnoCode.Domain.Entities;
using InnoCode.PostgresSql.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InnoCode.PostgresSql.Data.EntityConfigurations;

internal sealed class UserLoginEntityConfiguration : IEntityTypeConfiguration<UserLoginEntity>
{
    public void Configure(EntityTypeBuilder<UserLoginEntity> builder)
    {
        builder.ToTable(MetaData.Table.TableName, table => table.HasComment("Contain user logins"));

        builder
            .HasOne(userLogin => userLogin.User)
            .WithMany(user => user.UserLogins)
            .HasForeignKey(userLogin => userLogin.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }

    internal static class MetaData
    {
        internal static class Table
        {
            internal static readonly string TableName = "UserLogins";

            internal static readonly string TableSchema = CommonConstant.DatabaseSchemaName.DEFAULT;
        }
    }
}
