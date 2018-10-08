using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveRightCommand : Command
{
    public override void Execute(Actor actor)
    {
        actor.MoveRight();
    }

    public override void Undo(Actor actor)
    {
        actor.MoveLeft();
    }
}
