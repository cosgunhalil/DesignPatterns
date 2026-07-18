namespace DesignPatterns.Command
{
    /// <summary>
    /// A command whose effect can be reverted. Kept separate from
    /// <see cref="ICommand"/> because not every action is undoable
    /// (a fired projectile, a physics impulse, a network call).
    /// </summary>
    public interface IUndoableCommand : ICommand
    {
        void Undo();
    }
}
