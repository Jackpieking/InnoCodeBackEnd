using System;
using InnoCode.Domain.Common;
using Microsoft.AspNetCore.Identity;

namespace InnoCode.Domain.Entities;

public sealed class UserTokenEntity : IdentityUserToken<Guid>, IEntity
{
    public DateTime ExpiredAt { get; set; }

    public UserEntity User { get; set; }

    public static class MetaData
    {
        public static class Property
        {
            public static class Value
            {
                public const int MinLength = 1;
            }
        }
    }
}
