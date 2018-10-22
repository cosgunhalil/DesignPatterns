using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Atletic : Player {

    public override void SetupComponents()
    {
        physicsComponents = new PhysicsComponent[]
        {
            new JumpAbility(this, 1.5f, 250f),
            new RunAbility(this, 12f, .5f)
        };
    }

}
