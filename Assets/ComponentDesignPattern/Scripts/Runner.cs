using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Runner : Player {

    public override void SetupComponents()
    {
        physicsComponents = new PhysicsComponent[]
        {
            new RunAbility(this, 25f, 1f)
        };
    }

}
