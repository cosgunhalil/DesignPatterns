using DesignPatterns.Builder.Sample;
using NUnit.Framework;

namespace DesignPatterns.Builder.Tests
{
    public class ValidationTests
    {
        [Test]
        public void MissingName_Throws()
        {
            var ex = Assert.Throws<BuilderValidationException>(() => new CharacterBuilder().Build());

            Assert.Contains("Name is required.", (System.Collections.ICollection)ex.Errors);
        }

        [Test]
        public void NonPositiveHealth_Throws()
        {
            var ex = Assert.Throws<BuilderValidationException>(
                () => new CharacterBuilder().SetName("X").SetHealth(0).Build());

            Assert.Contains("Health must be greater than zero.", (System.Collections.ICollection)ex.Errors);
        }

        [Test]
        public void NegativeSpeed_Throws()
        {
            var ex = Assert.Throws<BuilderValidationException>(
                () => new CharacterBuilder().SetName("X").SetSpeed(-1f).Build());

            Assert.Contains("Speed cannot be negative.", (System.Collections.ICollection)ex.Errors);
        }

        [Test]
        public void MultipleProblems_AreAllReportedAtOnce()
        {
            var ex = Assert.Throws<BuilderValidationException>(
                () => new CharacterBuilder().SetHealth(-5).Build());

            Assert.AreEqual(2, ex.Errors.Count, "both the missing name and bad health should be reported");
        }

        [Test]
        public void ValidConfiguration_DoesNotThrow()
        {
            Assert.DoesNotThrow(() => new CharacterBuilder().SetName("Valid").Build());
        }

        [Test]
        public void Rebuilding_AfterFixingErrors_Succeeds()
        {
            var builder = new CharacterBuilder().SetHealth(0);
            Assert.Throws<BuilderValidationException>(() => builder.Build());

            // Errors are recomputed each Build(), not accumulated across calls.
            var character = builder.SetName("Fixed").SetHealth(10).Build();
            Assert.AreEqual("Fixed", character.Name);
        }
    }
}
