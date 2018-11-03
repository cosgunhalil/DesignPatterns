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

    private SoldierFSM fsm;
    private SpriteRenderer Renderer;

    public void Init()
    {
        Renderer = GetComponent<SpriteRenderer>();
        fsm = new SoldierFSM();
        fsm.OnGraphicChanged += SetGraphic;

        SetPhysics(true,0);
    }

    public void SoldierUpdate()
    {
        fsm.CallStateExecute();
    }

    public void SetPhysics(bool isGround, float velX)
    {
        isOnGround = isGround;
        velocityX = velX;

        fsm.SetBool("onGround", isGround);
        fsm.SetFloat("velocityX", velX);
    }

    public void SetIsDive(bool isDive)
    {
        fsm.SetBool("isDiveStarted", isDive);
    }

    public void SetJump(bool isJump)
    {
        fsm.SetBool("isJump", isJump);
    }

    public void SetSuperKick(bool isSuperKick)
    {
        fsm.SetBool("isSuperKickStarted", isSuperKick);
    }

    public void SetGraphic(int graphicIndex)
    {
        Renderer.sprite = Graphics[graphicIndex]; 
    }
}
