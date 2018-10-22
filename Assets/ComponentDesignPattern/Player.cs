using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public abstract class Player : MonoBehaviour {

    protected Transform playerTransform;
    protected Rigidbody2D playerRigidbody;
    protected PhysicsComponent[] physicsComponents;

    private Vector2 targetPoint;

	public void PlayerStart ()
    {
        SetupComponents();
        playerTransform = GetComponent<Transform>();
        playerRigidbody = GetComponent<Rigidbody2D>();
        targetPoint = new Vector2(50, 0);
	}

    public abstract void SetupComponents();

    public void PlayerUpdate ()
    {
        if (!CheckIsFinished())
        {
            for (int i = 0; i < physicsComponents.Length; i++)
            {
                physicsComponents[i].ComponentUpdate();
            }
        }
	}

    private bool CheckIsFinished()
    {
        return playerTransform.position.x >= targetPoint.x;
    }

    public Transform GetTransform()
    {
        return playerTransform;
    }

    public Rigidbody2D GetRigidbody()
    {
        return playerRigidbody;
    }
}
