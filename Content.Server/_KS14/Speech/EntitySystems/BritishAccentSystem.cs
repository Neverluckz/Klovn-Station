using Content.Server.Speech.Components;
using Robust.Shared.Random;

namespace Content.Server.Speech.EntitySystems;

public sealed class BritishAccentComponentAccentSystem : EntitySystem
{
    [Dependency] private readonly IRobustRandom _random = default!;

    private static readonly IReadOnlyDictionary<string, string[]> Replacements = new Dictionary<string, string[]>()
    {
        { "Hello",  ["Wagwan", "Oi"] },
        { "Hey",  ["Wagwan", "Oi"] },
        { "Pedo",  ["Nonce"] },
        { "Security",  ["Feds", "Pigs", "Coppas"] },
        { "Sec",  ["Feds", "Pigs", "Coppas"] },
        { "Spesos",  ["quid"] },
        { "Dude", ["bloke"]},
        { "Good", ["Peng"]},
        { "oh no", ["Oh bloody hell!"]},
        { "bad", ["shite"]},
        { "yes?", ["go on theeen", "go on mate"]},
        { "Conservative", ["Tori", "Tory"]},
        { "Cybersun", ["dodgey muckers"]},
        { "captain", ["prime minister"]},
        { "interdyne pharmaceuticals", ["department of health"]},
        { "interdyne", ["department of health"]},
        { "changeling", ["weird looking bloke", "shambler", "odd feller"]},
        { "ling", ["weird looking bloke", "shambler", "odd feller"]},
        { "wizard", ["magical bloke"]},
        { "nukie", ["damn terrorist", "terrorist"]},
        { "syndicate", ["foreigners"]},
        { "heretic", ["fackin witch"]},
        { "fuc", ["fek"]}, // 'fucking' -> 'fekking'
        { "thief", ["slimey bastard"]},
        { "coin", ["penny"]},
        { "no,", ["nah mate,"]},
        { "no.", ["nah mate."]},
        { "clone", ["copycat"]},
        { "devil", ["rich bloke"]},
        { "cultist", ["sum weird religious bloke", "odd religious fellow"]},
        { "t",  ["'"] },
    };

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<BritishAccentComponent, AccentGetEvent>(OnAccent);
    }

    public string Accentuate(string message)
    {
        foreach (var (word, repl) in Replacements)
            message = message.Replace(word, _random.Pick(repl));

        return message;
    }

    private void OnAccent(Entity<BritishAccentComponent> entity, ref AccentGetEvent args)
        => args.Message = Accentuate(args.Message);
}
