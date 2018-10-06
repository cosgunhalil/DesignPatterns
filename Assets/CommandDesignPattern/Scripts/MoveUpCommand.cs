using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveUpCommand : Command
{
    public override void Execute(Actor actor)
    {
        actor.MoveUp();
    }

    public override void Undo(Actor actor)
    {
        actor.MoveDown();
    }
}
