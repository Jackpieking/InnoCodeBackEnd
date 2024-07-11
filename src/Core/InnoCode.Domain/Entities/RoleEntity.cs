using System;
using System.Collections.Generic;
using InnoCode.Domain.Common;
using Microsoft.AspNetCore.Identity;

namespace InnoCode.Domain.Entities;

public sealed class RoleEntity : IdentityRole<Guid>, IEntity
{
    public RoleDetailEntity RoleDetail { get; set; }

    public IEnumerable<UserRoleEntity> UserRoles { get; set; }

    public IEnumerable<RoleClaimEntity> RoleClaims { get; set; }
}
