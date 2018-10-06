using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveLeftCommand : Command
{
    public override void Execute(Actor actor)
    {
        actor.MoveLeft();
    }

    public override void Undo(Actor actor)
    {
        actor.MoveRight();
    }
}
