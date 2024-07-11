using System;
using InnoCode.Domain.Common;
using Microsoft.AspNetCore.Identity;

namespace InnoCode.Domain.Entities;

public sealed class UserClaimEntity : IdentityUserClaim<Guid>, IEntity
{
    public UserEntity User { get; set; }
}
