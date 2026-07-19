namespace DesignPatterns.Builder.Sample
{
    /// <summary>
    /// A weak melee grunt. Directors take a fresh builder and drive it through a
    /// fixed recipe, so callers get a finished, consistent preset without
    /// repeating the assembly steps at every call site.
    /// </summary>
    public sealed class GruntDirector : IDirector<CharacterBuilder, Character>
    {
        public Character Construct(CharacterBuilder builder) =>
            builder
                .SetName("Grunt")
                .SetHealth(50)
                .SetSpeed(2f)
                .AddAbility("Melee")
                .AddLoot("Copper Coin")
                .Build();
    }

    /// <summary>A ranged attacker: less health, more speed, a bow.</summary>
    public sealed class ArcherDirector : IDirector<CharacterBuilder, Character>
    {
        public Character Construct(CharacterBuilder builder) =>
            builder
                .SetName("Archer")
                .SetHealth(40)
                .SetSpeed(3f)
                .AddAbility("Ranged")
                .AddAbility("Volley")
                .AddLoot("Arrow Bundle")
                .Build();
    }

    /// <summary>A tanky boss with multiple abilities and rich loot.</summary>
    public sealed class BossDirector : IDirector<CharacterBuilder, Character>
    {
        public Character Construct(CharacterBuilder builder) =>
            builder
                .SetName("Boss")
                .SetHealth(500)
                .SetSpeed(1.5f)
                .AddAbility("Melee")
                .AddAbility("Cleave")
                .AddAbility("Enrage")
                .AddLoot("Legendary Sword")
                .AddLoot("Gold Chest")
                .Build();
    }
}
