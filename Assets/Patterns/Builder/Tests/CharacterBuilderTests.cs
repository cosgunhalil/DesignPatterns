using DesignPatterns.Builder.Sample;
using NUnit.Framework;

namespace DesignPatterns.Builder.Tests
{
    public class CharacterBuilderTests
    {
        [Test]
        public void FluentMethods_PreserveConcreteBuilderType()
        {
            var builder = new CharacterBuilder();

            // CRTP payoff: each step is typed as CharacterBuilder, not the base.
            CharacterBuilder chained = builder.SetName("A").SetHealth(10).SetSpeed(1f).AddAbility("x");

            Assert.AreSame(builder, chained);
        }

        [Test]
        public void Build_ProducesConfiguredValues()
        {
            var character = new CharacterBuilder()
                .SetName("Hero")
                .SetHealth(120)
                .SetSpeed(2.5f)
                .AddAbility("Dash")
                .AddLoot("Potion")
                .Build();

            Assert.AreEqual("Hero", character.Name);
            Assert.AreEqual(120, character.Health);
            Assert.AreEqual(2.5f, character.Speed);
            CollectionAssert.AreEqual(new[] { "Dash" }, character.Abilities);
            CollectionAssert.AreEqual(new[] { "Potion" }, character.Loot);
        }

        [Test]
        public void Defaults_AreAppliedWhenNotSet()
        {
            var character = new CharacterBuilder().SetName("Default").Build();

            Assert.AreEqual(100, character.Health);
            Assert.AreEqual(1f, character.Speed);
            Assert.IsEmpty(character.Abilities);
            Assert.IsEmpty(character.Loot);
        }

        [Test]
        public void AddAbility_Accumulates()
        {
            var character = new CharacterBuilder()
                .SetName("Multi")
                .AddAbility("A")
                .AddAbility("B")
                .AddAbility("C")
                .Build();

            CollectionAssert.AreEqual(new[] { "A", "B", "C" }, character.Abilities);
        }

        [Test]
        public void MutatingBuilderAfterBuild_DoesNotAffectBuiltProduct()
        {
            var builder = new CharacterBuilder().SetName("Snapshot").AddAbility("First");
            var character = builder.Build();

            builder.AddAbility("Second");

            CollectionAssert.AreEqual(new[] { "First" }, character.Abilities,
                "a built Character must be an independent snapshot");
        }

        [Test]
        public void SeparateBuilds_FromDistinctBuilders_DoNotShareState()
        {
            var a = new CharacterBuilder().SetName("A").AddAbility("Ax").Build();
            var b = new CharacterBuilder().SetName("B").AddAbility("Bx").Build();

            CollectionAssert.AreEqual(new[] { "Ax" }, a.Abilities);
            CollectionAssert.AreEqual(new[] { "Bx" }, b.Abilities);
        }
    }
}
