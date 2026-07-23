using System;
using NUnit.Framework;
using DesignPatterns.Decorator.Sample;

namespace DesignPatterns.Decorator.Tests
{
    public class DamageDecoratorTests
    {
        [Test]
        public void Weapon_ReturnsItsBaseDamage()
        {
            Assert.AreEqual(10, new Weapon("Sword", 10).GetDamage());
        }

        [Test]
        public void Sharpen_AddsFlatBonus()
        {
            var sharpened = new SharpenEnchantment(new Weapon("Sword", 10), 3);

            Assert.AreEqual(13, sharpened.GetDamage());
        }

        [Test]
        public void Fire_AddsFlatDamage()
        {
            var fiery = new FireEnchantment(new Weapon("Sword", 10), 5);

            Assert.AreEqual(15, fiery.GetDamage());
        }

        [Test]
        public void Crit_MultipliesDamage()
        {
            var crit = new CriticalStrike(new Weapon("Sword", 10), 2);

            Assert.AreEqual(20, crit.GetDamage());
        }

        [Test]
        public void StackedEnchantments_ComposeInWrappingOrder()
        {
            // ((10 + 3) + 5) * 2 = 36
            IDamageSource source = new CriticalStrike(
                new FireEnchantment(
                    new SharpenEnchantment(new Weapon("Sword", 10))));

            Assert.AreEqual(36, source.GetDamage());
        }

        [Test]
        public void Order_ChangesTheResult()
        {
            var critInner = new SharpenEnchantment(new CriticalStrike(new Weapon("Axe", 10))); // 10*2 + 3 = 23
            var critOuter = new CriticalStrike(new SharpenEnchantment(new Weapon("Axe", 10))); // (10+3)*2 = 26

            Assert.AreEqual(23, critInner.GetDamage());
            Assert.AreEqual(26, critOuter.GetDamage());
            Assert.AreNotEqual(critInner.GetDamage(), critOuter.GetDamage());
        }

        [Test]
        public void SameDecorator_CanBeStackedMoreThanOnce()
        {
            var doubleFire = new FireEnchantment(new FireEnchantment(new Weapon("Torch", 4), 5), 5);

            Assert.AreEqual(14, doubleFire.GetDamage());
        }

        [Test]
        public void Describe_ReflectsTheComposition()
        {
            IDamageSource source = new CriticalStrike(new SharpenEnchantment(new Weapon("Sword", 10)));

            Assert.AreEqual("(Sword +Sharpen(3)) xCrit(2)", source.Describe());
        }

        [Test]
        public void Enchantment_NullInner_Throws()
        {
            Assert.Throws<ArgumentNullException>(() => new FireEnchantment(null));
        }
    }
}
