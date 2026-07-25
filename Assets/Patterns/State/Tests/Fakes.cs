using System.Collections.Generic;

namespace DesignPatterns.State.Tests
{
    /// <summary>
    /// A state that appends its lifecycle calls to a shared log, so tests can
    /// assert the exact Enter/Update/Exit ordering across transitions. The
    /// context is just the log list.
    /// </summary>
    internal sealed class RecordingState : State<List<string>>
    {
        private readonly string _name;

        public RecordingState(string name)
        {
            _name = name;
        }

        public override void Enter(StateMachine<List<string>> machine) => machine.Context.Add($"{_name}.Enter");

        public override void Update(StateMachine<List<string>> machine) => machine.Context.Add($"{_name}.Update");

        public override void Exit(StateMachine<List<string>> machine) => machine.Context.Add($"{_name}.Exit");
    }
}
