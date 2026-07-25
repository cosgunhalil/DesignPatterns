using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace DesignPatterns.State.Tests
{
    public class StateMachineTests
    {
        private List<string> _log;

        [SetUp]
        public void SetUp()
        {
            _log = new List<string>();
        }

        [Test]
        public void Constructor_EntersTheInitialState()
        {
            var machine = new StateMachine<List<string>>(_log, new RecordingState("A"));

            CollectionAssert.AreEqual(new[] { "A.Enter" }, _log);
            Assert.IsTrue(machine.IsInState<RecordingState>());
        }

        [Test]
        public void Update_TicksTheCurrentState()
        {
            var machine = new StateMachine<List<string>>(_log, new RecordingState("A"));
            _log.Clear();

            machine.Update();

            CollectionAssert.AreEqual(new[] { "A.Update" }, _log);
        }

        [Test]
        public void ChangeState_ExitsOldThenEntersNew_InThatOrder()
        {
            var machine = new StateMachine<List<string>>(_log, new RecordingState("A"));
            _log.Clear();

            machine.ChangeState(new RecordingState("B"));

            CollectionAssert.AreEqual(new[] { "A.Exit", "B.Enter" }, _log);
        }

        [Test]
        public void ChangeState_UpdatesCurrentState()
        {
            var machine = new StateMachine<List<string>>(_log, new RecordingState("A"));
            var b = new RecordingState("B");

            machine.ChangeState(b);

            Assert.AreSame(b, machine.CurrentState);
        }

        [Test]
        public void ChangeState_RaisesStateChangedWithFromAndTo()
        {
            var a = new RecordingState("A");
            var machine = new StateMachine<List<string>>(_log, a);
            var b = new RecordingState("B");

            IState<List<string>> from = null;
            IState<List<string>> to = null;
            machine.StateChanged += (previous, next) => { from = previous; to = next; };

            machine.ChangeState(b);

            Assert.AreSame(a, from);
            Assert.AreSame(b, to);
        }

        [Test]
        public void Constructor_NullInitialState_Throws()
        {
            Assert.Throws<ArgumentNullException>(() => new StateMachine<List<string>>(_log, null));
        }

        [Test]
        public void ChangeState_Null_Throws()
        {
            var machine = new StateMachine<List<string>>(_log, new RecordingState("A"));

            Assert.Throws<ArgumentNullException>(() => machine.ChangeState(null));
        }
    }
}
