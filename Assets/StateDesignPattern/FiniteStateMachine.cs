using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FiniteStateMachine {

    public Soldier animationContainer;
    private Dictionary<StateType, State> states;
    private State currentState;

    public FiniteStateMachine(Soldier animationContainer)
    {
        this.animationContainer = animationContainer;

        states = new Dictionary<StateType, State>()
        {
            { StateType.idle, new IdleState(this)},
            { StateType.run, new RunState   (this)},
            { StateType.jump, new JumpState(this)},
            { StateType.dive, new DiveState(this)},
            { StateType.superKick, new SuperKickState(this)}
        };

        currentState = states[StateType.idle];
        CallStateEnter();
    }

    public void SetState(StateType state)
    {
        if (currentState != states[state])
        {
            CallStateExit();
        }

        currentState = states[state];
        CallStateEnter();
    }

    private void CallStateEnter()
    {
        currentState.Enter();
    }

    private void CallStateExit()
    {
        currentState.Exit();
    }

    public void CallStateExecute()
    {
        currentState.Execute();
    }
}

public enum StateType
{
    idle,
    run,
    jump,
    dive,
    superKick
}
