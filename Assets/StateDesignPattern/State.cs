using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State {

    public abstract void Enter();
    public abstract void Execute();
    public abstract void Exit();

    protected FiniteStateMachine stateMachine;

    public State(FiniteStateMachine fsm)
    {
        this.stateMachine = fsm;
    }
}
