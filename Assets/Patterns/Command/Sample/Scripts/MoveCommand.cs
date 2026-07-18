using UnityEngine;

namespace DesignPatterns.Command.Sample
{
    /// <summary>
    /// Undoable: each instance captures the position it moved from at Execute
    /// time, so Undo restores exactly that state. This is why the input handler
    /// creates a NEW command per key press instead of reusing a shared instance.
    /// </summary>
    public class MoveCommand : UndoableCommand<Player>
    {
        private readonly Vector3 _delta;
        private Vector3 _positionBeforeMove;

        public MoveCommand(Player player, Vector3 delta) : base(player)
        {
            _delta = delta;
        }

        public override void Execute()
        {
            _positionBeforeMove = Receiver.Position;
            Receiver.Move(_delta);
            Debug.Log($"<color=cyan>Move {_delta}</color>");
        }

        public override void Undo()
        {
            Receiver.Position = _positionBeforeMove;
            Debug.Log($"<color=orange>Undo move → back to {_positionBeforeMove}</color>");
        }
    }
}
