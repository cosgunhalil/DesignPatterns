using UnityEngine;

namespace DesignPatterns.Builder.Sample
{
    /// <summary>
    /// Entry point. Press Play — it builds one custom character fluently, the
    /// three enemy presets via directors, and deliberately triggers a
    /// validation failure so you can see the builder reject a bad configuration.
    /// </summary>
    public sealed class BuilderPatternDemo : MonoBehaviour
    {
        private void Start()
        {
            // 1) Fluent, ad-hoc construction — the builder reads like a sentence.
            var hero = new CharacterBuilder()
                .SetName("Hero")
                .SetHealth(120)
                .SetSpeed(2.5f)
                .AddAbility("Dash")
                .AddAbility("Parry")
                .AddLoot("Health Potion")
                .Build();
            Debug.Log($"<color=cyan>Custom:</color> {hero}");

            // 2) Reusable presets via directors — same builder, canned recipes.
            LogPreset("Grunt", new GruntDirector().Construct(new CharacterBuilder()));
            LogPreset("Archer", new ArcherDirector().Construct(new CharacterBuilder()));
            LogPreset("Boss", new BossDirector().Construct(new CharacterBuilder()));

            // 3) Validation: a nameless, zero-health character is rejected with
            //    all problems reported at once.
            try
            {
                new CharacterBuilder().SetHealth(0).Build();
            }
            catch (BuilderValidationException ex)
            {
                Debug.Log($"<color=orange>Rejected as expected:</color> {string.Join(" | ", ex.Errors)}");
            }
        }

        private static void LogPreset(string label, Character character) =>
            Debug.Log($"<color=lime>{label}:</color> {character}");
    }
}
