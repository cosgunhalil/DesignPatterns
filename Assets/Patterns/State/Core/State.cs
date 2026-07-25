namespace DesignPatterns.State
{
    /// <summary>
    /// Convenience base so a concrete state overrides only the lifecycle hooks it
    /// cares about — most states don't need all three of Enter/Update/Exit.
    /// </summary>
    /// <typeparam name="TContext">The domain object the states operate on.</typeparam>
    public abstract class State<TContext> : IState<TContext>
    {
        public virtual void Enter(StateMachine<TContext> machine)
        {
        }

        public virtual void Update(StateMachine<TContext> machine)
        {
        }

        public virtual void Exit(StateMachine<TContext> machine)
        {
        }
    }
}
