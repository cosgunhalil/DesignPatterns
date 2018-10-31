using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Simulator : MonoBehaviour {

    public Soldier animationContainer;

    private Action[] actions;
    private int currentActionIndex = 0;

    // Use this for initialization
    void Start () {

        animationContainer.AnimContainerStart();

        actions = new Action[]
        {
            ()=>animationContainer.SetPhysics(true, 0),//idle
            ()=>{
                animationContainer.SetInput(InputType.pressedUpButton);//jump start
                animationContainer.SetPhysics(false, 0);
            },
            ()=>{
                animationContainer.SetInput(InputType.none);//jump end
                animationContainer.SetPhysics(true, 0);
            },
            ()=>animationContainer.SetPhysics(true, 1),//run start
            ()=>{
                animationContainer.SetInput(InputType.pressedUpButton);//jump start
                animationContainer.SetPhysics(false, 1);
            },
            ()=>{
                animationContainer.SetInput(InputType.pressedDownButton);//dive start
                animationContainer.SetPhysics(false, 1);
            },
            ()=>{
                animationContainer.SetInput(InputType.none);//dive end
                animationContainer.SetPhysics(true, 1);
            },
            ()=>{
                animationContainer.SetInput(InputType.none);//jump end
                animationContainer.SetPhysics(true, 0);
            },
            ()=>{
                animationContainer.SetInput(InputType.none);//jump end
                animationContainer.SetPhysics(true, 1);
            },
            ()=>animationContainer.SetPhysics(true,1),//run state
            ()=>{
                animationContainer.SetInput(InputType.pressedAButton);//super kick start
                animationContainer.SetPhysics(true, 1);
            },
            ()=>{
                animationContainer.SetInput(InputType.none);//super kick end
                animationContainer.SetPhysics(true, 0);
            },
        };
    }

    // Update is called once per frame
    void Update ()
    {
        animationContainer.AnimContainerUpdate();

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            NextAction();
        }
	}

    public void NextAction()
    {
        currentActionIndex++;
        if (currentActionIndex > actions.Length - 1)
        {
            currentActionIndex = actions.Length - 1;
        }

        ExecuteCurrentAction();
    }

    public void ExecuteCurrentAction()
    {
        actions[currentActionIndex]();
    }
}
