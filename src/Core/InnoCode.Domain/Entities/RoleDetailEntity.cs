using System;
using InnoCode.Domain.Common;

namespace InnoCode.Domain.Entities;

public sealed class RoleDetailEntity : IEntity, ITemporarilyRemovedEntity
{
    public Guid RoleId { get; set; }

    public bool IsTemporarilyRemoved { get; set; }

    public RoleEntity Role { get; set; }
}
