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
        stateMachine.animationContainer.SetGraphic(4);
    }

    public override void Execute()
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
    }

    public override void Exit()
    {
        throw new System.NotImplementedException();
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
