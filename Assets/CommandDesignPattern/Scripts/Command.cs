using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Command {

    public abstract void Execute(Actor actor);

    public virtual void Undo(Actor actor)
    {

    }
	
}
