using System;
using InnoCode.Domain.Common;
using Microsoft.AspNetCore.Identity;

namespace InnoCode.Domain.Entities;

public sealed class UserRoleEntity : IdentityUserRole<Guid>, IEntity
{
    public RoleEntity Role { get; set; }

    public UserEntity User { get; set; }
}
