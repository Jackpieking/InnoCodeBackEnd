using InnoCode.Domain.Entities;
using InnoCode.PostgresSql.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InnoCode.PostgresSql.Data.EntityConfigurations;

internal sealed class UserTokenEntityConfiguration : IEntityTypeConfiguration<UserTokenEntity>
{
    public void Configure(EntityTypeBuilder<UserTokenEntity> builder)
    {
        builder.ToTable(
            MetaData.Table.TableName,
            MetaData.Table.TableSchema,
            table => table.HasComment("Contain user tokens.")
        );

        builder
            .Property(builder => builder.ExpiredAt)
            .HasColumnType(CommonConstant.DatabaseNativeType.TIMESTAMPTZ)
            .HasColumnName(MetaData.Property.ExpiredAt.ColumnName)
            .IsRequired(true);

        builder
            .HasOne(userToken => userToken.User)
            .WithMany(user => user.UserTokens)
            .HasForeignKey(userToken => userToken.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }

    internal static class MetaData
    {
        internal static class Table
        {
            internal static readonly string TableName = "user_token";

            internal static readonly string TableSchema = CommonConstant.DatabaseSchemaName.DEFAULT;
        }

        internal static class Property
        {
            internal static class ExpiredAt
            {
                internal static readonly string ColumnName = nameof(UserTokenEntity.ExpiredAt);
            }
        }
    }
}
