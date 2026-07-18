namespace DesignPatterns.Command
{
    /// <summary>
    /// Base for receiver-bound commands that support undo. Concrete commands
    /// must capture whatever state <see cref="Undo"/> needs at Execute time —
    /// which is why an undoable command is one instance per invocation,
    /// never a shared/reused object.
    /// </summary>
    /// <typeparam name="TReceiver">The object the command operates on.</typeparam>
    public abstract class UndoableCommand<TReceiver> : Command<TReceiver>, IUndoableCommand
        where TReceiver : class
    {
        protected UndoableCommand(TReceiver receiver) : base(receiver)
        {
        }

        public abstract void Undo();
    }
}
