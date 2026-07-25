using System;

namespace DesignPatterns.State
{
    /// <summary>
    /// Generic finite state machine. It holds the shared <see cref="Context"/> and
    /// the current state, and drives the Exit→Enter handshake on every transition.
    /// States decide when to change (by calling <see cref="ChangeState"/>); the
    /// machine just orchestrates — no giant conditional anywhere.
    /// </summary>
    /// <typeparam name="TContext">The domain object the states operate on.</typeparam>
    public sealed class StateMachine<TContext>
    {
        /// <summary>Raised after a transition, with (previousState, newState).</summary>
        public event Action<IState<TContext>, IState<TContext>> StateChanged;

        public TContext Context { get; }
        public IState<TContext> CurrentState { get; private set; }

        public StateMachine(TContext context, IState<TContext> initialState)
        {
            Context = context;
            CurrentState = initialState ?? throw new ArgumentNullException(nameof(initialState));
            CurrentState.Enter(this);
        }

        /// <summary>Leave the current state and enter <paramref name="next"/>, in that order.</summary>
        public void ChangeState(IState<TContext> next)
        {
            if (next == null)
            {
                throw new ArgumentNullException(nameof(next));
            }

            var previous = CurrentState;
            previous.Exit(this);
            CurrentState = next;
            next.Enter(this);
            StateChanged?.Invoke(previous, next);
        }

        /// <summary>Tick the current state.</summary>
        public void Update() => CurrentState.Update(this);

        /// <summary>True when the current state is of the given type — handy for queries and tests.</summary>
        public bool IsInState<TState>() where TState : IState<TContext> => CurrentState is TState;
    }
}
