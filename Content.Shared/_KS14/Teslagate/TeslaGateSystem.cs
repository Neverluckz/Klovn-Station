using Content.Shared.Damage;
using Content.Shared.Damage.Prototypes;
using Content.Shared.Power;
using Content.Shared.Power.EntitySystems;
using Robust.Shared.Audio.Systems;
using Robust.Shared.Physics.Components;
using Robust.Shared.Physics.Events;
using Robust.Shared.Physics.Systems;
using Robust.Shared.Prototypes;
using Robust.Shared.Timing;

namespace Content.Shared.KS14.TeslaGate;

public abstract class SharedTeslaGateSystem : EntitySystem
{
    [Dependency] private readonly SharedPowerReceiverSystem _powerReceiverSystem = default!;
    [Dependency] private readonly IGameTiming _gameTiming = default!;
    [Dependency] private readonly SharedAppearanceSystem _appearanceSystem = default!;
    [Dependency] private readonly SharedPointLightSystem _pointLight = default!;

    public override void Initialize()
    {
        base.Initialize();
    }

    public bool IsFinishedShocking(TeslaGateComponent teslaGateComponent) => _gameTiming.CurTime > teslaGateComponent.LastShockTime + teslaGateComponent.ShockLength;

    protected void UpdateAppearance(Entity<TeslaGateComponent> teslaGate, bool active)
    {
        var (uid, teslaGateComponent) = teslaGate;

        _appearanceSystem.SetData(teslaGate, TeslaGateVisuals.ShockingState, active);
        _pointLight.SetEnabled(uid, active);
    }
    private void OnPowerChange(Entity<TeslaGateComponent> teslaGate, ref PowerChangedEvent args)
    {
        //var (uid, teslaGateComponent) = teslaGate;
        UpdateAppearance(teslaGate, args.Powered);
        Dirty(teslaGate);
    }
}
