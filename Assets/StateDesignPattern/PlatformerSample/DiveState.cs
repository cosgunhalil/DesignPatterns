using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiveState : State {
    public DiveState(FiniteStateMachine fsm) : base(fsm)
    {
        this.stateMachine = fsm;
    }

    public override void Enter()
    {
        Debug.Log("Enter Dive State");
        stateMachine.ChangePlayingGraphicIndex(4);
    }

    public override void Execute()
    {
        if (stateMachine.GetBool("onGround"))
        {
            var velocityX = stateMachine.GetFloat("velocityX");
            if (velocityX == 0)
            {
                stateMachine.SetState("idle");
            }
            else if (velocityX > 0)
            {
                stateMachine.SetState("run");
            }
        }
    }

    public override void Exit()
    {
        Debug.Log("Exit from Dive State");
    }
}
