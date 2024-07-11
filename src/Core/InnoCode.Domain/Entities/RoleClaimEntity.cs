using System;
using InnoCode.Domain.Common;
using Microsoft.AspNetCore.Identity;

namespace InnoCode.Domain.Entities;

public sealed class RoleClaimEntity : IdentityRoleClaim<Guid>, IEntity
{
    public RoleEntity Role { get; set; }
}
