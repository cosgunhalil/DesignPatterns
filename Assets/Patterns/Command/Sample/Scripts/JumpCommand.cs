using UnityEngine;

namespace DesignPatterns.Command.Sample
{
    /// <summary>
    /// Deliberately NOT undoable: a physics impulse cannot be cleanly reverted.
    /// The invoker still executes it but keeps it out of the undo history —
    /// press Z after jumping and you'll see the jump is skipped.
    /// </summary>
    public class JumpCommand : Command<Player>
    {
        private readonly float _force;

        public JumpCommand(Player player, float force) : base(player)
        {
            _force = force;
        }

        public override void Execute()
        {
            Receiver.Jump(_force);
            Debug.Log("<color=lime>Jump!</color>");
        }
    }
}
