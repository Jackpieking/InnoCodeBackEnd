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
            name: MetaData.Table.TableName,
            schema: MetaData.Table.TableSchema,
            buildAction: table => table.HasComment(comment: "Contain user tokens.")
        );

        builder
            .Property(propertyExpression: builder => builder.ExpiredAt)
            .HasColumnType(typeName: CommonConstant.DatabaseNativeType.TIMESTAMPTZ)
            .HasColumnName(name: MetaData.Property.ExpiredAt.ColumnName)
            .IsRequired(required: true);

        builder
            .HasOne(navigationExpression: userToken => userToken.User)
            .WithMany(navigationExpression: user => user.UserTokens)
            .HasForeignKey(foreignKeyExpression: userToken => userToken.UserId)
            .OnDelete(deleteBehavior: DeleteBehavior.Cascade);
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
