using Content.Server.Ghost.Components;
using Content.Shared.Ghost;
using Robust.Server.GameObjects;
using Robust.Server.Player;
using Robust.Shared.GameObjects;
using Robust.Shared.IoC;
using Robust.Shared.Log;
using Robust.Shared.Prototypes;
using System.Linq;
using Content.Shared.Movement.Events;

namespace Content.Server.Ghost;

public sealed class CustomGhostSystem : EntitySystem
{
    [Dependency] private readonly IPrototypeManager _prototypeManager = default!;
    [Dependency] private readonly IPlayerManager _playerManager = default!;

    public override void Initialize()
    {
        base.Initialize();
        SubscribeLocalEvent<PlayerAttachedEvent>(OnPlayerAttached);
    }

    private void OnPlayerAttached(PlayerAttachedEvent args)
    {
        if (!args.Entity.HasComponent<GhostComponent>())
            return;

        var player = args.Player;
        var ckey = player.Session.Name;

        // Ищем прототип кастомного призрака для этого игрока
        var customGhostPrototype = _prototypeManager.EnumeratePrototypes<CustomGhostPrototype>()
            .FirstOrDefault(p => p.Ckey == ckey);

        if (customGhostPrototype == null)
            return;

        // Добавляем компонент кастомного призрака
        var customGhost = EntityManager.AddComponent<CustomGhostComponent>(args.Entity);
        customGhost.GhostName = customGhostPrototype.GhostName;
        customGhost.GhostDescription = customGhostPrototype.GhostDescription;
        customGhost.Alpha = customGhostPrototype.Alpha;

        // Обновляем спрайт
        if (EntityManager.TryGetComponent<SpriteComponent>(args.Entity, out var sprite))
        {
            sprite.LayerSetSprite(0, customGhostPrototype.Sprite);
        }
    }
}
