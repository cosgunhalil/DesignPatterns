using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuperKickState : State {
    private float playTime;
    private float currentTime;
    public SuperKickState(FiniteStateMachine fsm) : base(fsm)
    {
        stateMachine = fsm;
        playTime = 2f;
    }

    public override void Enter()
    {
        Debug.Log("Enter Super Kick State");
        stateMachine.animationContainer.SetGraphic(5);
        currentTime = 0;
    }

    public override void Execute()
    {
        currentTime += Time.deltaTime;
        if (currentTime > playTime)
        {
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
        Debug.Log("<color=white>Super Kick Animation is playing</color>");
        
    }

    public override void Exit()
    {
        Debug.Log("Exiting From Super Kick State");
    }
}
