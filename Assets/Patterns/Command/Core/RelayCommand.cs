using System;

namespace DesignPatterns.Command
{
    /// <summary>
    /// Defines a command from a delegate instead of a dedicated class — handy
    /// for one-off actions. Once an action needs captured state or undo,
    /// promote it to a real command class.
    /// </summary>
    public sealed class RelayCommand : ICommand
    {
        private readonly Action _execute;

        public RelayCommand(Action execute)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
        }

        public void Execute() => _execute();
    }

    /// <summary>
    /// Delegate-based command that carries a context value. Passing the context
    /// explicitly (instead of capturing it in a closure) lets a static lambda be
    /// reused without per-call allocations.
    /// </summary>
    /// <typeparam name="TContext">The value handed to the delegate on execution.</typeparam>
    public sealed class RelayCommand<TContext> : ICommand
    {
        private readonly Action<TContext> _execute;
        private readonly TContext _context;

        public RelayCommand(Action<TContext> execute, TContext context)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _context = context;
        }

        public void Execute() => _execute(_context);
    }
}
