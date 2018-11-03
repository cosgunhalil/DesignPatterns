using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Simulator : MonoBehaviour {

    public Soldier soldier;

    private Action[] actions;
    private int currentActionIndex = 0;

    // Use this for initialization
    void Start () {

        soldier.Init();

        actions = new Action[]
        {
            ()=>soldier.SetPhysics(true, 0),//idle
            ()=>{
                soldier.SetJump(true);//jump start
                soldier.SetPhysics(false, 0);
            },
            ()=>{
                soldier.SetJump(false);//jump end
                soldier.SetPhysics(true, 0);
            },
            ()=>soldier.SetPhysics(true, 1),//run start
            ()=>{
                soldier.SetJump(true);//jump start
                soldier.SetPhysics(false, 1);
            },
            ()=>{
                soldier.SetIsDive(true);//dive start
                soldier.SetPhysics(false, 1);
            },
            ()=>{
                soldier.SetIsDive(false);//dive end
                soldier.SetPhysics(true, 1);
                soldier.SetJump(false);//jump end    
            },
            ()=>soldier.SetPhysics(true, 0),
            ()=>soldier.SetPhysics(true,1),//run state
            ()=>{
                soldier.SetSuperKick(true);//super kick start
                soldier.SetPhysics(true, 1);
            },
            ()=>{
                soldier.SetSuperKick(false);//super kick end
                soldier.SetPhysics(true, 0);
            },
        };
    }

    void Update ()
    {
        soldier.SoldierUpdate();

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
