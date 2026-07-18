namespace DesignPatterns.Command
{
    /// <summary>
    /// The heart of the pattern: an action captured as an object.
    /// Whoever executes it needs no knowledge of what it does or whom it acts on.
    /// </summary>
    public interface ICommand
    {
        void Execute();
    }
}
