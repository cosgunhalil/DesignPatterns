using System;
using NUnit.Framework;

namespace DesignPatterns.Factory.Tests
{
    public class ParameterizedKeyedFactoryTests
    {
        private KeyedFactory<string, int, string> _factory;

        [SetUp]
        public void SetUp()
        {
            _factory = new KeyedFactory<string, int, string>();
        }

        [Test]
        public void Create_PassesTheArgumentToTheCreator()
        {
            _factory.Register("stars", count => new string('*', count));

            Assert.AreEqual("****", _factory.Create("stars", 4));
        }

        [Test]
        public void Create_UnknownKey_Throws()
        {
            Assert.Throws<UnknownFactoryKeyException>(() => _factory.Create("missing", 1));
        }

        [Test]
        public void TryCreate_PassesArgumentWhenKnown()
        {
            _factory.Register("dots", count => new string('.', count));

            Assert.IsTrue(_factory.TryCreate("dots", 3, out var product));
            Assert.AreEqual("...", product);
        }

        [Test]
        public void TryCreate_UnknownKey_ReturnsFalse()
        {
            Assert.IsFalse(_factory.TryCreate("missing", 5, out var product));
            Assert.IsNull(product);
        }

        [Test]
        public void DifferentArguments_ProduceDifferentProducts()
        {
            _factory.Register("hash", count => new string('#', count));

            Assert.AreEqual("#", _factory.Create("hash", 1));
            Assert.AreEqual("###", _factory.Create("hash", 3));
        }

        [Test]
        public void Register_FactoryObject_Works()
        {
            _factory.Register("rep", new RepeatFactory('z'));

            Assert.AreEqual("zzz", _factory.Create("rep", 3));
        }

        [Test]
        public void Register_NullCreator_Throws()
        {
            Assert.Throws<ArgumentNullException>(() => _factory.Register("k", (Func<int, string>)null));
        }
    }
}
