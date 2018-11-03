using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : State {

    public IdleState(FiniteStateMachine fsm) : base(fsm)
    {
        stateMachine = fsm;
    }

    public override void Enter()
    {
        Debug.Log("Enter Idle State");
        stateMachine.ChangePlayingGraphicIndex(0);
    }

    public override void Execute()
    {
        Debug.Log("<color=blue>Idle Animation is playing</color>");
        var velocityX = stateMachine.GetFloat("velocityX");
        if (stateMachine.GetBool("onGround"))
        {
            if (velocityX == 0)
            {
                if (stateMachine.GetBool("isJump"))
                {
                    stateMachine.SetState("jump");
                }
            }
            else if (velocityX > 0)
            {
                stateMachine.SetState("run");
            }
        }
        else
        {
            stateMachine.SetState("jump");
        }
        
    }

    public override void Exit()
    {
        Debug.Log("Exit from Idle State");
    }
}
