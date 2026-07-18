using System.Collections.Generic;

namespace DesignPatterns.Command.Tests
{
    /// <summary>Undoable test double that records every Execute/Undo call.</summary>
    internal sealed class FakeUndoableCommand : IUndoableCommand
    {
        private readonly string _name;
        private readonly List<string> _callLog;

        public int ExecuteCount { get; private set; }
        public int UndoCount { get; private set; }

        public FakeUndoableCommand(string name = "command", List<string> callLog = null)
        {
            _name = name;
            _callLog = callLog;
        }

        public void Execute()
        {
            ExecuteCount++;
            _callLog?.Add($"{_name}.Execute");
        }

        public void Undo()
        {
            UndoCount++;
            _callLog?.Add($"{_name}.Undo");
        }
    }

    /// <summary>Non-undoable test double.</summary>
    internal sealed class FakeCommand : ICommand
    {
        private readonly string _name;
        private readonly List<string> _callLog;

        public int ExecuteCount { get; private set; }

        public FakeCommand(string name = "command", List<string> callLog = null)
        {
            _name = name;
            _callLog = callLog;
        }

        public void Execute()
        {
            ExecuteCount++;
            _callLog?.Add($"{_name}.Execute");
        }
    }
}
