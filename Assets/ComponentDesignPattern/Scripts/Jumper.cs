using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jumper : Player {

    public override void SetupComponents()
    {
        physicsComponents = new PhysicsComponent[]
        {
            new JumpAbility(this, 1f, 600f)
        };
    }
}
