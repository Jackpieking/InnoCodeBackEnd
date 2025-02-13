namespace InnoCode.Domain.Common;

/// <summary>
///     Represent the base interface that all entity classes
///     that need to track the temporarily removed must inherit from.
/// </summary>
public interface ITemporarilyRemovedEntity
{
    bool IsTemporarilyRemoved { get; set; }
}
