using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunState : State {

    private float frameChangeRate;
    private float time;

    private int[] graphicIndexArray;
    private int currentGraphicIndex;

    public RunState(FiniteStateMachine fsm) : base(fsm)
    {
        stateMachine = fsm;
        frameChangeRate = .1f;
        graphicIndexArray = new int[] { 2, 3 };
    }

    public override void Enter()
    {
        Debug.Log("Enter Running State");
        currentGraphicIndex = 0;
        stateMachine.animationContainer.SetGraphic(currentGraphicIndex);
        time = 0f;
    }

    public override void Execute()
    {
        Debug.Log("<color=red>Running Animation is playing</color>");
        if (stateMachine.animationContainer.IsOnGround())
        {
            if (stateMachine.animationContainer.GetInputType() == InputType.pressedAButton)
            {
                stateMachine.SetState(StateType.superKick);
            }
            else if (stateMachine.animationContainer.GetInputType() == InputType.pressedUpButton)
            {
                stateMachine.SetState(StateType.jump);
            }
            else if (stateMachine.animationContainer.GetVelocityX() == 0)
            {
                stateMachine.SetState(StateType.idle);
            }
        }
        else
        {
            stateMachine.SetState(StateType.jump);
        }

        time += Time.deltaTime;

        if (time > frameChangeRate)
        {
            stateMachine.animationContainer.SetGraphic(graphicIndexArray[GetNextGrahicIndex()]);
            time = 0f;
        }

    }

    private int GetNextGrahicIndex()
    {
        currentGraphicIndex++;
        if (currentGraphicIndex > graphicIndexArray.Length - 1)
        {
            currentGraphicIndex = 0;
        }

        return currentGraphicIndex;
    }

    public override void Exit()
    {
        Debug.Log("Exiting From Running State");
    }
}
