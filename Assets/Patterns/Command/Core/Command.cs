using System;

namespace DesignPatterns.Command
{
    /// <summary>
    /// Convenience base for the common case of a command acting on a single
    /// receiver. The generic parameter keeps concrete commands strongly typed
    /// to the receiver they need — no casting, no shared mutable state.
    /// </summary>
    /// <typeparam name="TReceiver">The object the command operates on.</typeparam>
    public abstract class Command<TReceiver> : ICommand where TReceiver : class
    {
        protected TReceiver Receiver { get; }

        protected Command(TReceiver receiver)
        {
            Receiver = receiver ?? throw new ArgumentNullException(nameof(receiver));
        }

        public abstract void Execute();
    }
}
