using System.Collections.Generic;

namespace DesignPatterns.Builder.Sample
{
    /// <summary>
    /// The immutable product. Once built it never changes: every property is
    /// init-only and the collections are exposed as read-only. It has no
    /// constructor telescoping and no "half-built" state — the builder is the
    /// only way to make one, and it hands back a finished object.
    /// </summary>
    public sealed class Character
    {
        public string Name { get; init; }
        public int Health { get; init; }
        public float Speed { get; init; }
        public IReadOnlyList<string> Abilities { get; init; }
        public IReadOnlyList<string> Loot { get; init; }

        public override string ToString()
        {
            var abilities = Abilities.Count == 0 ? "none" : string.Join(", ", Abilities);
            var loot = Loot.Count == 0 ? "none" : string.Join(", ", Loot);
            return $"{Name} (HP {Health}, spd {Speed}) — abilities: {abilities}; loot: {loot}";
        }
    }
}
