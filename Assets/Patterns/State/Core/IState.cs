namespace DesignPatterns.State
{
    /// <summary>
    /// One state of a state machine. The machine hands itself to each call so a
    /// state can read the shared context (<c>machine.Context</c>) and request a
    /// transition (<c>machine.ChangeState(...)</c>). Behavior that would
    /// otherwise be a sprawling <c>switch (currentState)</c> lives here, one
    /// state per class.
    /// </summary>
    /// <typeparam name="TContext">The domain object the states operate on.</typeparam>
    public interface IState<TContext>
    {
        /// <summary>Runs once when this state becomes active.</summary>
        void Enter(StateMachine<TContext> machine);

        /// <summary>Runs each tick while this state is active; the usual place to trigger transitions.</summary>
        void Update(StateMachine<TContext> machine);

        /// <summary>Runs once as this state is left, before the next state's Enter.</summary>
        void Exit(StateMachine<TContext> machine);
    }
}
