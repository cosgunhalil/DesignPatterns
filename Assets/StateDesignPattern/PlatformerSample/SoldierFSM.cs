using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierFSM : FiniteStateMachine {

    public SoldierFSM()
    {
        Setup();
    }

    public override void Setup()
    {
        states = new Dictionary<string, State>()
        {
            { "idle", new IdleState(this)},
            { "run", new RunState   (this)},
            { "jump", new JumpState(this)},
            { "dive", new DiveState(this)},
            { "superKick", new SuperKickState(this)}
        };

        boolVariables = new Dictionary<string, bool>
        {
            { "onGround", false },
            { "isDiveStarted", false },
            { "isJump", false},
            { "isSuperKickStarted", false},
        };

        floatVariables = new Dictionary<string, float>
        {
            {"velocityX", 0 }
        };

        currentState = states["idle"];
        CallStateEnter();
    }

}
