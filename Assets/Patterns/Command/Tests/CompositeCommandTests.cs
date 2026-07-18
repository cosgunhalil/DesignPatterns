using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace DesignPatterns.Command.Tests
{
    public class CompositeCommandTests
    {
        [Test]
        public void Execute_RunsChildrenInOrder()
        {
            var callLog = new List<string>();
            var composite = new CompositeCommand(
                new FakeUndoableCommand("first", callLog),
                new FakeUndoableCommand("second", callLog));

            composite.Execute();

            CollectionAssert.AreEqual(new[] { "first.Execute", "second.Execute" }, callLog);
        }

        [Test]
        public void Undo_RunsChildrenInReverseOrder()
        {
            var callLog = new List<string>();
            var composite = new CompositeCommand(
                new FakeUndoableCommand("first", callLog),
                new FakeUndoableCommand("second", callLog));
            composite.Execute();
            callLog.Clear();

            composite.Undo();

            CollectionAssert.AreEqual(new[] { "second.Undo", "first.Undo" }, callLog);
        }

        [Test]
        public void Undo_SkipsNonUndoableChildren()
        {
            var callLog = new List<string>();
            var composite = new CompositeCommand(
                new FakeUndoableCommand("undoable", callLog),
                new FakeCommand("plain", callLog));
            composite.Execute();
            callLog.Clear();

            composite.Undo();

            CollectionAssert.AreEqual(new[] { "undoable.Undo" }, callLog);
        }

        [Test]
        public void Add_ReturnsItself_ForChaining()
        {
            var composite = new CompositeCommand();
            var callLog = new List<string>();

            composite
                .Add(new FakeUndoableCommand("first", callLog))
                .Add(new FakeUndoableCommand("second", callLog))
                .Execute();

            CollectionAssert.AreEqual(new[] { "first.Execute", "second.Execute" }, callLog);
        }

        [Test]
        public void Add_NullCommand_Throws()
        {
            Assert.Throws<ArgumentNullException>(() => new CompositeCommand().Add(null));
        }
    }
}
