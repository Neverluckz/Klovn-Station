using Content.Server.Speech.Components;
using Robust.Shared.Random;

namespace Content.Server.Speech.EntitySystems;

public sealed class BigotAccentComponentAccentSystem : EntitySystem
{
    [Dependency] private readonly IRobustRandom _random = default!;

    private static readonly IReadOnlyDictionary<string, string[]> Replacements = new Dictionary<string, string[]>()
    {
        { "Hello",  ["YOU"] },
        { "Hi",  ["LISTEN HERE"] },
        { "Lizard",  ["LIZZZAAARRDDD"] },
        { "Moth",  ["LAMPER"] },
        { "Cargo",  ["MONEY HOARDING", "THOSE WHO KNOW"] },
        { "QM",  ["THE ELITE", "NT INSIDER"] },
        { "Security", ["SHITSEC!!!"]},
        { "Sec", ["Fucking shitsec..."]},
        { "I",  ["I, A TRUE REAL PERSON,"] },
    };

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<BigotAccentComponent, AccentGetEvent>(OnAccent);
    }

    public string Accentuate(string message)
    {
        foreach (var (word, repl) in Replacements)
            message = message.Replace(word, _random.Pick(repl));

        return message;
    }

    private void OnAccent(Entity<BigotAccentComponent> entity, ref AccentGetEvent args)
        => args.Message = Accentuate(args.Message);
}
