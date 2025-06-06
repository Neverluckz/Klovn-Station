using Content.Shared.KS14.TeslaGate;
using Content.Shared.Silicons.StationAi;
using Robust.Shared.Utility;

namespace Content.Client.Silicons.StationAi;

public sealed partial class StationAiSystem
{
    private void InitializeTeslagateInteraction()
    {
        SubscribeLocalEvent<TeslaGateComponent, GetStationAiRadialEvent>(OnTeslagateGetRadial);
    }

    private void OnTeslagateGetRadial(Entity<TeslaGateComponent> ent, ref GetStationAiRadialEvent args)
    {
        var comp = ent.Comp;
        args.Actions.Add(new StationAiRadial()
        {
            Tooltip = comp.Enabled
                ? Loc.GetString("electrify-door-off")
                : Loc.GetString("electrify-door-on"),
            Sprite = new SpriteSpecifier.Texture(new ResPath("/Textures/Interface/VerbIcons/zap.svg.192dpi.png")),
            Event = new StationAiTeslaGateEvent() { Enabled = !comp.Enabled }
        });
    }
}
