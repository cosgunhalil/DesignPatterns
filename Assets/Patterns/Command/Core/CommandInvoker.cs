using System;
using System.Collections.Generic;

namespace DesignPatterns.Command
{
    /// <summary>
    /// Runs commands and keeps an undo/redo history of the undoable ones.
    /// The invoker is the only place that knows about history; commands and
    /// receivers stay oblivious to it.
    /// </summary>
    public class CommandInvoker
    {
        private readonly Stack<IUndoableCommand> _undoHistory = new();
        private readonly Stack<IUndoableCommand> _redoHistory = new();

        public bool CanUndo => _undoHistory.Count > 0;
        public bool CanRedo => _redoHistory.Count > 0;

        /// <summary>
        /// Executes the command. Undoable commands are recorded; anything else
        /// runs without leaving a trace in the history.
        /// </summary>
        public void Execute(ICommand command)
        {
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            command.Execute();

            if (command is IUndoableCommand undoable)
            {
                _undoHistory.Push(undoable);
                _redoHistory.Clear(); // a new action invalidates the redo branch
            }
        }

        /// <summary>Reverts the most recent undoable command. Returns false when the history is empty.</summary>
        public bool Undo()
        {
            if (!CanUndo)
            {
                return false;
            }

            var command = _undoHistory.Pop();
            command.Undo();
            _redoHistory.Push(command);
            return true;
        }

        /// <summary>Re-executes the most recently undone command. Returns false when there is nothing to redo.</summary>
        public bool Redo()
        {
            if (!CanRedo)
            {
                return false;
            }

            var command = _redoHistory.Pop();
            command.Execute();
            _undoHistory.Push(command);
            return true;
        }
    }
}
