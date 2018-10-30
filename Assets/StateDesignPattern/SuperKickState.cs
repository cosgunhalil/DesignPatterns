using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuperKickState : State {

    public SuperKickState(FiniteStateMachine fsm) : base(fsm)
    {
        stateMachine = fsm;
    }

    public override void Enter()
    {
        Debug.Log("Enter Super Kick State");
        stateMachine.animationContainer.SetGraphic(5);
    }

    public override void Execute()
    {
        Debug.Log("<color=white>Super Kick Animation is playing</color>");
        if (stateMachine.animationContainer.IsOnGround())
        {
            if (stateMachine.animationContainer.GetVelocityX() == 0)
            {
                stateMachine.SetState(StateType.idle);
            }
            else if (stateMachine.animationContainer.GetVelocityX() > 0)
            {
                stateMachine.SetState(StateType.run);
            }
        }
        else
        {
            stateMachine.SetState(StateType.jump);
        }
    }

    public override void Exit()
    {
        Debug.Log("Exiting From Super Kick State");
    }
}
