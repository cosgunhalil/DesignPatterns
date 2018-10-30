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
        stateMachine.animationContainer.SetGraphic(0);
    }

    public override void Execute()
    {
        Debug.Log("<color=blue>Idle Animation is playing</color>");

        if (stateMachine.animationContainer.GetVelocityX() == 0)
        {
            if (stateMachine.animationContainer.GetInputType() == InputType.pressedUpButton)
            {
                stateMachine.SetState(StateType.jump);
            }
        }
        else if (stateMachine.animationContainer.GetVelocityX() > 0)
        {
            stateMachine.SetState(StateType.run);
        }
    }

    public override void Exit()
    {
        Debug.Log("Exit from Idle State");
    }
}
