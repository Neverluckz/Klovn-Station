


using Content.Shared.Damage;
using Robust.Shared.Audio;
using Robust.Shared.GameStates;
using Robust.Shared.Serialization;
using Robust.Shared.Utility;

namespace Content.Shared.KS14.TeslaGate;

/// <summary>
/// This is used for...
/// </summary>
[RegisterComponent, NetworkedComponent]
public sealed partial class TeslaGateComponent : Component
{
    public float PulseAccumulator = 0f;

    // this is just to make it easier to work with
    public bool CurrentlyShocking = false;

    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public HashSet<EntityUid> ThingsBeingShocked = new();

    [ViewVariables(VVAccess.ReadOnly)]
    public TimeSpan LastShockTime = TimeSpan.MinValue;

    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public bool Enabled = false;

    /// <summary>
    /// Whether this tesla gate's aux wire is cut.
    /// </summary>
    [DataField]
    public bool IsAuxWireCut = false;

    /// <summary>
    /// Whether this tesla gate's pulse interval is hacked via wires.
    /// </summary>
    [DataField]
    public bool IsIntervalHacked = false;

    /// <summary>
    /// Whether this tesla gate is hacked to force it to be on.
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadOnly)]
    public bool IsForceHacked = false;

    /// <summary>
    /// Which alert levels does the tesla gate automatically turn on at?
    /// Must be lowercase.
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadOnly)]
    public List<string> EnabledAlertLevels = new() { "red", "violet", "epsilon", "delta", "gamma", "omicron" };


    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public TimeSpan PulseInterval = TimeSpan.FromSeconds(3);

    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public TimeSpan ShockLength = TimeSpan.FromSeconds(1);


    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public TimeSpan DefaultPulseInterval = TimeSpan.FromSeconds(3);
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public TimeSpan HackedPulseInterval = TimeSpan.FromSeconds(1.5);


    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public SoundSpecifier? ShockSound = new SoundPathSpecifier(new ResPath("/Audio/Effects/Lightning/lightningshock.ogg"));

    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public SoundSpecifier? StartingSound = new SoundPathSpecifier(new ResPath("/Audio/Effects/poster_being_set.ogg"));



    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public float ShockDamage = 75f;
}

[Serializable, NetSerializable]
public enum TeslaGateVisuals : byte { ShockingState }

[Serializable, NetSerializable]
public enum TeslaGateVisualState : byte
{
    Active,
    Inactive,
}

[Serializable, NetSerializable]
public enum TeslaGateSafetyWireKey : byte
{
    StatusKey,
}

[Serializable, NetSerializable]
public enum TeslaGateForceWireKey : byte
{
    StatusKey,
}

public enum TeslaGateAuxWireKey : byte
{
    StatusKey,
}

