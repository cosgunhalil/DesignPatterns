using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace DesignPatterns.Command.Tests
{
    public class CommandInvokerTests
    {
        private CommandInvoker _invoker;

        [SetUp]
        public void SetUp()
        {
            _invoker = new CommandInvoker();
        }

        [Test]
        public void Execute_InvokesTheCommand()
        {
            var command = new FakeUndoableCommand();

            _invoker.Execute(command);

            Assert.AreEqual(1, command.ExecuteCount);
        }

        [Test]
        public void Execute_NullCommand_Throws()
        {
            Assert.Throws<ArgumentNullException>(() => _invoker.Execute(null));
        }

        [Test]
        public void Execute_UndoableCommand_IsRecordedInHistory()
        {
            _invoker.Execute(new FakeUndoableCommand());

            Assert.IsTrue(_invoker.CanUndo);
        }

        [Test]
        public void Execute_NonUndoableCommand_LeavesNoHistory()
        {
            _invoker.Execute(new FakeCommand());

            Assert.IsFalse(_invoker.CanUndo);
        }

        [Test]
        public void Undo_RevertsTheLastCommand_AndEnablesRedo()
        {
            var command = new FakeUndoableCommand();
            _invoker.Execute(command);

            var result = _invoker.Undo();

            Assert.IsTrue(result);
            Assert.AreEqual(1, command.UndoCount);
            Assert.IsFalse(_invoker.CanUndo);
            Assert.IsTrue(_invoker.CanRedo);
        }

        [Test]
        public void Undo_RunsInReverseExecutionOrder()
        {
            var callLog = new List<string>();
            _invoker.Execute(new FakeUndoableCommand("first", callLog));
            _invoker.Execute(new FakeUndoableCommand("second", callLog));

            _invoker.Undo();
            _invoker.Undo();

            CollectionAssert.AreEqual(
                new[] { "first.Execute", "second.Execute", "second.Undo", "first.Undo" },
                callLog);
        }

        [Test]
        public void Redo_ReexecutesTheUndoneCommand()
        {
            var command = new FakeUndoableCommand();
            _invoker.Execute(command);
            _invoker.Undo();

            var result = _invoker.Redo();

            Assert.IsTrue(result);
            Assert.AreEqual(2, command.ExecuteCount);
            Assert.IsTrue(_invoker.CanUndo);
            Assert.IsFalse(_invoker.CanRedo);
        }

        [Test]
        public void Execute_AfterUndo_ClearsTheRedoHistory()
        {
            _invoker.Execute(new FakeUndoableCommand());
            _invoker.Undo();

            _invoker.Execute(new FakeUndoableCommand());

            Assert.IsFalse(_invoker.CanRedo);
        }

        [Test]
        public void Undo_WithEmptyHistory_ReturnsFalse()
        {
            Assert.IsFalse(_invoker.Undo());
        }

        [Test]
        public void Redo_WithEmptyHistory_ReturnsFalse()
        {
            Assert.IsFalse(_invoker.Redo());
        }
    }
}
