using DesignPatterns.Builder.Sample;
using NUnit.Framework;

namespace DesignPatterns.Builder.Tests
{
    public class DirectorTests
    {
        [Test]
        public void GruntDirector_ProducesTheGruntPreset()
        {
            var grunt = new GruntDirector().Construct(new CharacterBuilder());

            Assert.AreEqual("Grunt", grunt.Name);
            Assert.AreEqual(50, grunt.Health);
            CollectionAssert.AreEqual(new[] { "Melee" }, grunt.Abilities);
        }

        [Test]
        public void BossDirector_ProducesTheBossPreset()
        {
            var boss = new BossDirector().Construct(new CharacterBuilder());

            Assert.AreEqual("Boss", boss.Name);
            Assert.AreEqual(500, boss.Health);
            CollectionAssert.AreEqual(new[] { "Melee", "Cleave", "Enrage" }, boss.Abilities);
            CollectionAssert.AreEqual(new[] { "Legendary Sword", "Gold Chest" }, boss.Loot);
        }

        [Test]
        public void Director_IsReusable_ProducingEquivalentButSeparateProducts()
        {
            var director = new ArcherDirector();

            var first = director.Construct(new CharacterBuilder());
            var second = director.Construct(new CharacterBuilder());

            Assert.AreNotSame(first, second);
            Assert.AreEqual(first.Name, second.Name);
            CollectionAssert.AreEqual(first.Abilities, second.Abilities);
        }

        [Test]
        public void Director_ReturnsProductThroughTheIDirectorContract()
        {
            IDirector<CharacterBuilder, Character> director = new GruntDirector();

            var product = director.Construct(new CharacterBuilder());

            Assert.IsInstanceOf<Character>(product);
        }
    }
}
