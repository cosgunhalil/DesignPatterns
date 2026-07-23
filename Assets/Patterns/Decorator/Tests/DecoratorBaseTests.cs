using System;
using NUnit.Framework;

namespace DesignPatterns.Decorator.Tests
{
    public class DecoratorBaseTests
    {
        [Test]
        public void Decorator_ForwardsToAndAugmentsTheWrappedComponent()
        {
            var loud = new LoudGreeter(new PlainGreeter());

            Assert.AreEqual("HI!", loud.Greet());
        }

        [Test]
        public void Decorators_NestToAnyDepth()
        {
            var twice = new LoudGreeter(new LoudGreeter(new PlainGreeter()));

            Assert.AreEqual("HI!!", twice.Greet());
        }

        [Test]
        public void Constructor_NullComponent_Throws()
        {
            Assert.Throws<ArgumentNullException>(() => new LoudGreeter(null));
        }

        [Test]
        public void Component_ExposesTheWrappedInstanceForUnwrapping()
        {
            var inner = new PlainGreeter();
            var loud = new LoudGreeter(inner);

            Assert.AreSame(inner, ((IDecorator<IGreeter>)loud).Component);
        }
    }
}
