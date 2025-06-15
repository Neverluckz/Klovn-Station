using Content.Shared.Ghost;
using Robust.Shared.GameObjects;
using Robust.Shared.Serialization.Manager.Attributes;

namespace Content.Server.Ghost;

[RegisterComponent]
public sealed class CustomGhostComponent : Component
{
    [DataField("ghostName")]
    public string GhostName { get; set; } = string.Empty;

    [DataField("ghostDescription")]
    public string GhostDescription { get; set; } = string.Empty;

    [DataField("alpha")]
    public float Alpha { get; set; } = 0.8f;
}
