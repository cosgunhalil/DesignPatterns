using System;
using System.Collections.Generic;

namespace DesignPatterns.Command
{
    /// <summary>
    /// A macro: one command made of many. Executes children in order and undoes
    /// them in reverse order, so the whole group behaves like a single history
    /// entry. Children that are not undoable are skipped on undo.
    /// </summary>
    public sealed class CompositeCommand : IUndoableCommand
    {
        private readonly List<ICommand> _commands = new();

        public CompositeCommand(params ICommand[] commands)
        {
            foreach (var command in commands)
            {
                Add(command);
            }
        }

        public CompositeCommand Add(ICommand command)
        {
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            _commands.Add(command);
            return this;
        }

        public void Execute()
        {
            foreach (var command in _commands)
            {
                command.Execute();
            }
        }

        public void Undo()
        {
            for (var i = _commands.Count - 1; i >= 0; i--)
            {
                if (_commands[i] is IUndoableCommand undoable)
                {
                    undoable.Undo();
                }
            }
        }
    }
}
