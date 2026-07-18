using System;
using NUnit.Framework;

namespace DesignPatterns.Command.Tests
{
    public class RelayCommandTests
    {
        [Test]
        public void Execute_InvokesTheDelegate()
        {
            var callCount = 0;
            var command = new RelayCommand(() => callCount++);

            command.Execute();

            Assert.AreEqual(1, callCount);
        }

        [Test]
        public void Constructor_NullDelegate_Throws()
        {
            Assert.Throws<ArgumentNullException>(() => new RelayCommand(null));
        }

        [Test]
        public void GenericExecute_PassesTheContext()
        {
            string received = null;
            var command = new RelayCommand<string>(context => received = context, "payload");

            command.Execute();

            Assert.AreEqual("payload", received);
        }

        [Test]
        public void GenericConstructor_NullDelegate_Throws()
        {
            Assert.Throws<ArgumentNullException>(() => new RelayCommand<int>(null, 42));
        }
    }
}
