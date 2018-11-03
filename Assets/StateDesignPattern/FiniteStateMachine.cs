using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class FiniteStateMachine {

    public abstract void Setup();
    public delegate void GraphicChanged(int graphicIndex);
    public GraphicChanged OnGraphicChanged;

    protected Dictionary<string, State> states;
    protected State currentState;

    protected Dictionary<string, bool> boolVariables;
    protected Dictionary<string, string> stringVariables;
    protected Dictionary<string, float> floatVariables;


    public void SetState(string state)
    {
        CallStateExit();
        currentState = states[state];
        CallStateEnter();
    }

    protected void CallStateEnter()
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

    public void SetBool(string key, bool val)
    {
        if (boolVariables.ContainsKey(key))
        {
            boolVariables[key] = val;
        }
    }

    public bool GetBool(string key)
    {
        if (boolVariables.ContainsKey(key))
        {
           return boolVariables[key];
        }
        else
        {
            Debug.Log("undefined key error! Please define the key " + key + " first!");
            return false;
        }
    }

    public void SetFloat(string key, float val)
    {
        if (floatVariables.ContainsKey(key))
        {
            floatVariables[key] = val;
        }
    }

    public float GetFloat(string key)
    {
        if (floatVariables.ContainsKey(key))
        {
            return floatVariables[key];
        }
        else
        {
            Debug.Log("undefined key error! Please define the key " + key + " first!");
            return 0;
        }
    }

    public void SetString(string key, string val)
    {
        if (stringVariables.ContainsKey(key))
        {
            stringVariables[key] = val;
        }
    }

    public string GetString(string key)
    {
        if (floatVariables.ContainsKey(key))
        {
            return stringVariables[key];
        }
        else
        {
            Debug.Log("undefined key error! Please define the key " + key + " first!");
            return string.Empty;
        }
    }

    public void ChangePlayingGraphicIndex(int index)
    {
        if (OnGraphicChanged != null)
        {
            OnGraphicChanged(index);
        }
    }

}
