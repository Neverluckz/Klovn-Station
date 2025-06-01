using System.Diagnostics.CodeAnalysis;
using Content.Server.Wires;
using Content.Shared.KS14.TeslaGate;
using Content.Shared.Wires;

namespace Content.Server.KS14.TeslaGate;

public sealed partial class TeslaGateSafetyWireAction : BaseToggleWireAction
{
    public override string Name { get; set; } = "wire-name-teslagate-safety";

    public override Color Color { get; set; } = Color.LightYellow;

    public override object? StatusKey { get; } = TeslaGateSafetyWireKey.StatusKey;

    public override void ToggleValue(EntityUid owner, bool setting)
    {
        if (!EntityManager.TryGetComponent<TeslaGateComponent>(owner, out var teslaGateComponent))
            return;

        var isHacked = !setting;
        teslaGateComponent.IsIntervalHacked = isHacked;

        if (isHacked)
            teslaGateComponent.PulseInterval = teslaGateComponent.HackedPulseInterval;
        else
            teslaGateComponent.PulseInterval = teslaGateComponent.DefaultPulseInterval;
    }

    public override bool GetValue(EntityUid owner) => EntityManager.TryGetComponent<TeslaGateComponent>(owner, out var teslaGateComponent)
        && teslaGateComponent.IsIntervalHacked;

    public override StatusLightState? GetLightState(Wire wire)
    {
        if (!EntityManager.TryGetComponent<TeslaGateComponent>(wire.Owner, out var teslaGateComponent) || !teslaGateComponent.Enabled)
            return StatusLightState.Off;

        return teslaGateComponent.IsIntervalHacked
            ? StatusLightState.BlinkingFast
            : StatusLightState.BlinkingSlow;
    }
}

public sealed partial class TeslaGateForceWireAction : BaseToggleWireAction
{
    private TeslaGateSystem _teslaGateSystem = default!;

    public override string Name { get; set; } = "wire-name-teslagate-alertsensor";

    public override Color Color { get; set; } = Color.Navy;

    public override object? StatusKey { get; } = TeslaGateForceWireKey.StatusKey;

    public override void Initialize()
    {
        base.Initialize();

        _teslaGateSystem = EntityManager.System<TeslaGateSystem>();
    }

    // i dont like replicating ugly code
    private bool TryGetGateEnt(Wire wire, [NotNullWhen(true)] out Entity<TeslaGateComponent>? teslaGate)
    {
        var owner = wire.Owner;
        if (!EntityManager.TryGetComponent<TeslaGateComponent>(owner, out var teslaGateComponent))
        {
            teslaGate = null;
            return false;
        }

        teslaGate = (owner, teslaGateComponent);
        return true;
    }

    public override void ToggleValue(EntityUid owner, bool setting) { }
    public override void Pulse(EntityUid user, Wire wire)
    {
        base.Pulse(user, wire);
        if (wire.IsCut)
            return;

        if (!TryGetGateEnt(wire, out var teslaGate))
            return;

        var (owner, teslaGateComponent) = teslaGate.Value;

        teslaGateComponent.IsForceHacked = !teslaGateComponent.IsForceHacked;
        if (teslaGateComponent.IsAuxWireCut && teslaGateComponent.IsForceHacked && teslaGateComponent.Enabled)
            _teslaGateSystem.Disable(teslaGate.Value);
    }

    public override bool GetValue(EntityUid owner) => EntityManager.TryGetComponent<TeslaGateComponent>(owner, out var teslaGateComponent)
        && teslaGateComponent.IsForceHacked;

    public override StatusLightState? GetLightState(Wire wire)
    {
        if (!EntityManager.TryGetComponent<TeslaGateComponent>(wire.Owner, out var teslaGateComponent))
            return StatusLightState.Off;

        return teslaGateComponent.IsForceHacked
            ? StatusLightState.On
            : StatusLightState.Off;
    }
}

public sealed partial class TeslaGateAuxWireAction : BaseToggleWireAction
{
    private TeslaGateSystem _teslaGateSystem = default!;

    public override string Name { get; set; } = "wire-name-teslagate-aux-current";

    public override Color Color { get; set; } = Color.Maroon;

    public override object? StatusKey { get; } = TeslaGateAuxWireKey.StatusKey;

    public override void Initialize()
    {
        base.Initialize();

        _teslaGateSystem = EntityManager.System<TeslaGateSystem>();
    }

    // i dont like replicating ugly code (however i did it again)
    private bool TryGetGateEnt(Wire wire, [NotNullWhen(true)] out Entity<TeslaGateComponent>? teslaGate)
    {
        var owner = wire.Owner;
        if (!EntityManager.TryGetComponent<TeslaGateComponent>(owner, out var teslaGateComponent))
        {
            teslaGate = null;
            return false;
        }

        teslaGate = (owner, teslaGateComponent);
        return true;
    }

    public override void ToggleValue(EntityUid owner, bool setting) { }

    public override bool Cut(EntityUid user, Wire wire)
    {
        if (!base.Cut(user, wire))
            return false;

        if (!TryGetGateEnt(wire, out var teslaGate))
            return false;

        var (_, teslaGateComponent) = teslaGate.Value;

        teslaGateComponent.IsAuxWireCut = true;
        if (teslaGateComponent.Enabled && teslaGateComponent.IsForceHacked)
            _teslaGateSystem.Disable(teslaGate.Value);

        return true;
    }

    public override bool Mend(EntityUid user, Wire wire)
    {
        if (!base.Mend(user, wire))
            return false;

        if (!TryGetGateEnt(wire, out var teslaGate))
            return false;

        var (_, teslaGateComponent) = teslaGate.Value;

        teslaGateComponent.IsAuxWireCut = false;
        if (!teslaGateComponent.Enabled && teslaGateComponent.IsForceHacked)
            _teslaGateSystem.Enable(teslaGate.Value);

        return true;
    }

    public override bool GetValue(EntityUid owner) => EntityManager.TryGetComponent<TeslaGateComponent>(owner, out var teslaGateComponent)
        && teslaGateComponent.Enabled;

    public override StatusLightState? GetLightState(Wire wire)
    {
        if (!EntityManager.TryGetComponent<TeslaGateComponent>(wire.Owner, out var teslaGateComponent))
            return StatusLightState.Off;

        return teslaGateComponent.Enabled
            ? StatusLightState.On
            : StatusLightState.Off;
    }
}
