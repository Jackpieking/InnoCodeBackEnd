using System;
using System.Collections.Generic;
using InnoCode.Domain.Common;
using Microsoft.AspNetCore.Identity;

namespace InnoCode.Domain.Entities;

public sealed class UserEntity : IdentityUser<Guid>, IEntity
{
    public IEnumerable<UserRoleEntity> UserRoles { get; set; }

    public IEnumerable<UserClaimEntity> UserClaims { get; set; }

    public IEnumerable<UserLoginEntity> UserLogins { get; set; }

    public IEnumerable<UserTokenEntity> UserTokens { get; set; }

    public static class MetaData
    {
        public static class Property
        {
            public static class UserName
            {
                public const int MaxLength = 50;

                public const int MinLength = 1;
            }

            public static class NormalizedUserName
            {
                public const int MaxLength = 100;

                public const int MinLength = 1;
            }

            public static class Email
            {
                public const int MaxLength = 100;

                public const int MinLength = 1;
            }

            public static class NormalizedEmail
            {
                public const int MaxLength = 100;

                public const int MinLength = 1;
            }

            public static class AccessFailedCount
            {
                public const int MinValue = default;

                public const int MaxValue = int.MaxValue;
            }

            public static class Password
            {
                public const int MaxLength = 100;

                public const int MinLength = 1;
            }
        }
    }
}
