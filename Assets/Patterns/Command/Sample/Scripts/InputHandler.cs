using System;
using System.Collections.Generic;
using UnityEngine;

namespace DesignPatterns.Command.Sample
{
    /// <summary>
    /// Maps keys to command factories. A key only knows how to CREATE a command,
    /// so rebinding controls is a single dictionary write — the classic reason
    /// games reach for the Command pattern. Factories (not instances) are stored
    /// because undoable commands need a fresh instance per press.
    /// </summary>
    public class InputHandler
    {
        private readonly Dictionary<KeyCode, Func<ICommand>> _bindings = new();

        public void Bind(KeyCode key, Func<ICommand> commandFactory)
        {
            _bindings[key] = commandFactory ?? throw new ArgumentNullException(nameof(commandFactory));
        }

        /// <summary>Returns a command for a key pressed this frame, or null.</summary>
        public ICommand HandleInput()
        {
            foreach (var binding in _bindings)
            {
                if (Input.GetKeyDown(binding.Key))
                {
                    return binding.Value();
                }
            }

            return null;
        }
    }
}
