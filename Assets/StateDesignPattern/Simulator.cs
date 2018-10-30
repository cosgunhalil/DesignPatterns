using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Simulator : MonoBehaviour {

    public Soldier animationContainer;

    private Action[] actions;

    // Use this for initialization
    void Start () {

        animationContainer.AnimContainerStart();

        actions = new Action[]
        {
            ()=>animationContainer.SetPhysics(true, 0),//idle
            ()=>animationContainer.SetPhysics(true, 0),//grounded
            ()=>animationContainer.SetPhysics(true, 1),//running
            ()=>animationContainer.SetPhysics(true,1),//run state
        };

    }

    // Update is called once per frame
    void Update ()
    {
        animationContainer.AnimContainerUpdate();	
	}
}
