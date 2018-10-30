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
        stateMachine.animationContainer.SetGraphic(1);
    }

    public override void Execute()
    {
        Debug.Log("<color=yellow>Jumping Animation is playing</color>");
        if (stateMachine.animationContainer.IsOnGround())
        {
            var playerVelocityX = stateMachine.animationContainer.GetVelocityX();
            if (playerVelocityX == 0)
            {
                stateMachine.SetState(StateType.idle);
            }
            else if (playerVelocityX > 0)
            {
                stateMachine.SetState(StateType.run);
            }
        }
        else if (stateMachine.animationContainer.GetInputType() == InputType.pressedDownButton)
        {
            stateMachine.SetState(StateType.dive);
        }
    }

    public override void Exit()
    {
        Debug.Log("Exit from Jumping State");
    }

}
