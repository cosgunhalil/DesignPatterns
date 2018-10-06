using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveDownCommand : Command
{
    public override void Execute(Actor actor)
    {
        actor.MoveDown();
    }

    public override void Undo(Actor actor)
    {
        actor.MoveUp();
    }
}
