using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soldier : MonoBehaviour {

    public Sprite[] Graphics;

    [SerializeField]
    private float velocityX;
    [SerializeField]
    private bool isOnGround;
    [SerializeField]
    private InputType inputType;

    private FiniteStateMachine fsm;
    private SpriteRenderer Renderer;

    public void AnimContainerStart()
    {
        Renderer = GetComponent<SpriteRenderer>();
        fsm = new FiniteStateMachine(this);
    }

    public void AnimContainerUpdate()
    {
        fsm.CallStateExecute();
    }

    public void SetInput(InputType inputType)
    {
        this.inputType = inputType;
    }

    public void SetPhysics(bool isGround, float velX)
    {
        isOnGround = isGround;
        velocityX = velX;
    }

    public void SetGraphic(int graphicIndex)
    {
        Renderer.sprite = Graphics[graphicIndex]; 
    }

    public bool IsOnGround()
    {
        return isOnGround;
    }

    public float GetVelocityX()
    {
        return velocityX;
    }

    public InputType GetInputType()
    {
        return inputType;
    }
}

public enum InputType
{
    none,
    pressedUpButton,
    pressedDownButton,
    pressedAButton
}
