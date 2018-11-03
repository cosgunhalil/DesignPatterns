using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpState : State {
    public JumpState(FiniteStateMachine fsm) : base(fsm)
    {
        stateMachine = fsm;
    }

    public override void Enter()
    {
        Debug.Log("Enter Jumping State");
        stateMachine.ChangePlayingGraphicIndex(1);
    }

    public override void Execute()
    {
        Debug.Log("<color=yellow>Jumping Animation is playing</color>");
        if (stateMachine.GetBool("onGround"))
        {
            var playerVelocityX = stateMachine.GetFloat("velocityX");
            if (playerVelocityX == 0)
            {
                stateMachine.SetState("idle");
            }
            else if (playerVelocityX > 0)
            {
                stateMachine.SetState("run");
            }
        }
        else if (stateMachine.GetBool("isDiveStarted"))
        {
            stateMachine.SetState("dive");
        }
    }

    public override void Exit()
    {
        Debug.Log("Exit from Jumping State");
    }

}
