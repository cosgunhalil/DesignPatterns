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
        stateMachine.ChangePlayingGraphicIndex(5);
        currentTime = 0;
    }

    public override void Execute()
    {
        currentTime += Time.deltaTime;
        if (currentTime > playTime)
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
            else
            {
                stateMachine.SetState("jump");//may be add a falling state
            }
        }
        Debug.Log("<color=white>Super Kick Animation is playing</color>");

    }

    public override void Exit()
    {
        Debug.Log("Exiting From Super Kick State");
    }
}
