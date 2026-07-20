using System;
using NUnit.Framework;

namespace DesignPatterns.Factory.Tests
{
    public class KeyedFactoryTests
    {
        private KeyedFactory<string, string> _factory;

        [SetUp]
        public void SetUp()
        {
            _factory = new KeyedFactory<string, string>();
        }

        [Test]
        public void Create_ReturnsProductFromRegisteredCreator()
        {
            _factory.Register("greet", () => "hello");

            Assert.AreEqual("hello", _factory.Create("greet"));
        }

        [Test]
        public void Create_UnknownKey_Throws()
        {
            var ex = Assert.Throws<UnknownFactoryKeyException>(() => _factory.Create("missing"));
            Assert.AreEqual("missing", ex.Key);
        }

        [Test]
        public void TryCreate_UnknownKey_ReturnsFalse()
        {
            var found = _factory.TryCreate("missing", out var product);

            Assert.IsFalse(found);
            Assert.IsNull(product);
        }

        [Test]
        public void TryCreate_KnownKey_ReturnsTrueAndProduct()
        {
            _factory.Register("x", () => "made");

            Assert.IsTrue(_factory.TryCreate("x", out var product));
            Assert.AreEqual("made", product);
        }

        [Test]
        public void CanCreate_ReflectsRegistration()
        {
            Assert.IsFalse(_factory.CanCreate("x"));
            _factory.Register("x", () => "y");
            Assert.IsTrue(_factory.CanCreate("x"));
        }

        [Test]
        public void Keys_ListsRegisteredKeys()
        {
            _factory.Register("a", () => "1").Register("b", () => "2");

            CollectionAssert.AreEquivalent(new[] { "a", "b" }, _factory.Keys);
        }

        [Test]
        public void Register_SameKeyTwice_LastWins()
        {
            _factory.Register("k", () => "first");
            _factory.Register("k", () => "second");

            Assert.AreEqual("second", _factory.Create("k"));
        }

        [Test]
        public void Register_NullCreator_Throws()
        {
            Assert.Throws<ArgumentNullException>(() => _factory.Register("k", (Func<string>)null));
        }

        [Test]
        public void Create_InvokesCreatorEachCall_ProducingFreshInstances()
        {
            var factory = new KeyedFactory<string, object>();
            factory.Register("obj", () => new object());

            Assert.AreNotSame(factory.Create("obj"), factory.Create("obj"));
        }

        [Test]
        public void Register_FactoryObject_Works()
        {
            _factory.Register("c", new ConstantFactory("from-object"));

            Assert.AreEqual("from-object", _factory.Create("c"));
        }

        [Test]
        public void CustomComparer_ControlsKeyMatching()
        {
            var factory = new KeyedFactory<string, string>(StringComparer.OrdinalIgnoreCase);
            factory.Register("Fire", () => "boom");

            Assert.AreEqual("boom", factory.Create("FIRE"));
        }
    }
}
