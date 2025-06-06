using Content.Shared.KS14.TeslaGate;
using Robust.Shared.Serialization;

namespace Content.Shared.Silicons.StationAi;

public abstract partial class SharedStationAiSystem
{
    [Dependency] private readonly SharedTeslaGateSystem _teslaGateSystem = default!;

    private void InitializeTeslagate()
    {
        SubscribeLocalEvent<TeslaGateComponent, StationAiTeslaGateEvent>(OnTeslagateEvent);
    }

    private void OnTeslagateEvent(Entity<TeslaGateComponent> teslaGate, ref StationAiTeslaGateEvent args)
    {
        var teslaGateComponent = teslaGate.Comp;
        if (args.Enabled == teslaGateComponent.Enabled)
            return;

        if (args.Enabled)
            _teslaGateSystem.Enable(teslaGate);
        else
            _teslaGateSystem.Disable(teslaGate);
    }
}

[Serializable, NetSerializable]
public sealed class StationAiTeslaGateEvent : BaseStationAiAction { public bool Enabled; }
