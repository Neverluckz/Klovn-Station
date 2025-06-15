using Robust.Shared.Prototypes;
using Robust.Shared.Serialization.Manager.Attributes;

namespace Content.Shared.Ghost;

[Prototype("customGhost")]
public sealed class CustomGhostPrototype : IPrototype
{
    [IdDataField]
    public string ID { get; } = default!;

    [DataField("ckey")]
    public string Ckey { get; } = string.Empty;

    [DataField("sprite")]
    public string Sprite { get; } = string.Empty;

    [DataField("ghostName")]
    public string GhostName { get; } = string.Empty;

    [DataField("ghostDescription")]
    public string GhostDescription { get; } = string.Empty;

    [DataField("alpha")]
    public float Alpha { get; } = 0.8f;
}
