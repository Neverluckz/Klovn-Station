using Content.Server.Ghost.Components;
using Content.Server.Ghost.Roles.Components;
using Content.Server.Ghost.Roles;
using Content.Server.Mind;
using Content.Server.Players;
using Content.Shared.Ghost;
using Content.Shared.Movement.Events;
using Content.Shared.Prototypes;
using Robust.Server.GameObjects;
using Robust.Server.Player;
using Robust.Shared.Prototypes;
using Robust.Shared.Timing;
using System.Linq;

namespace Content.Server.Ghost;

public sealed class CustomGhostSystem : EntitySystem
{
    [Dependency] private readonly IPrototypeManager _prototypeManager = default!;
    [Dependency] private readonly IPlayerManager _playerManager = default!;
    [Dependency] private readonly MindSystem _mindSystem = default!;
    [Dependency] private readonly GhostRoleSystem _ghostRoleSystem = default!;
    [Dependency] private readonly IGameTiming _gameTiming = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<GhostComponent, PlayerAttachedEvent>(OnPlayerAttached);
    }

    private void OnPlayerAttached(EntityUid uid, GhostComponent component, PlayerAttachedEvent args)
    {
        if (!TryComp<MindComponent>(uid, out var mindComp) || mindComp.Mind == null)
            return;

        var mind = mindComp.Mind;
        var player = _playerManager.GetSessionById(mind.UserId!.Value);
        if (player == null)
            return;

        // Проверяем, есть ли у игрока кастомный призрак
        var customGhost = _prototypeManager.EnumeratePrototypes<CustomGhostPrototype>()
            .FirstOrDefault(p => p.PlayerId == player.UserId);

        if (customGhost == null)
            return;

        // Применяем кастомный призрак
        var sprite = EnsureComp<SpriteComponent>(uid);
        sprite.LayerSetSprite(0, customGhost.Sprite);
        sprite.LayerSetColor(0, customGhost.Color);
    }
}
