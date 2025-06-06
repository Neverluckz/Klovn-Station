using Content.Shared.Examine;
using Content.Shared.Power;
using Robust.Shared.Timing;

namespace Content.Shared.KS14.TeslaGate;

public abstract class SharedTeslaGateSystem : EntitySystem
{
    [Dependency] private readonly IGameTiming _gameTiming = default!;
    [Dependency] private readonly SharedAppearanceSystem _appearanceSystem = default!;
    [Dependency] private readonly SharedPointLightSystem _pointLight = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<TeslaGateComponent, PowerChangedEvent>(OnPowerChange);
        SubscribeLocalEvent<TeslaGateComponent, ExaminedEvent>(OnExamined);
    }

    public bool IsFinishedShocking(TeslaGateComponent teslaGateComponent) => _gameTiming.CurTime > teslaGateComponent.LastShockTime + teslaGateComponent.ShockLength;

    protected void UpdateAppearance(Entity<TeslaGateComponent> teslaGate, bool active, TeslaGateVisualState state)
    {
        var (uid, teslaGateComponent) = teslaGate;

        _appearanceSystem.SetData(teslaGate, TeslaGateVisuals.ShockingState, state);
        _pointLight.SetEnabled(uid, active);

        Dirty(teslaGate);
    }

    private void OnExamined(Entity<TeslaGateComponent> teslaGate, ref ExaminedEvent args)
    {
        if (!args.IsInDetailsRange)
            return;

        args.PushMarkup(Loc.GetString("teslagate-on-examine", ("enabled", teslaGate.Comp.Enabled)), 1);
    }
    public abstract void OnPowerChange(Entity<TeslaGateComponent> teslaGate, ref PowerChangedEvent args);

    /// <summary>
    /// Enables the teslagate, playing the starting sound if it's powered.
    /// Safe to call even if the teslagate is already enabled.
    /// Doesn't do anything on client.
    /// </summary>
    public abstract void Enable(Entity<TeslaGateComponent> teslaGate);

    /// <summary>
    /// Disables the teslagate. Safe to call even if the teslagate is already disabled.
    /// Doesn't do anything on client.
    /// </summary>
    public abstract void Disable(Entity<TeslaGateComponent> teslaGate);
}
