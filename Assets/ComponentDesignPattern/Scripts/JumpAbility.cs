using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpAbility : PhysicsComponent {

    private float jumpFrequency;
    private float currentTime;
    private float jumpForce;

    public JumpAbility(Player player, float jumpFrequency, float jumpForce)
    {
        this.player = player;

        currentTime = 0f;
        this.jumpFrequency = jumpFrequency;
        this.jumpForce = jumpForce;
    }

    public override void ComponentUpdate()
    {
        currentTime += Time.deltaTime;
        if (currentTime >= jumpFrequency)
        {
            Jump();
            currentTime = 0;
        }
    }

    private void Jump()
    {
        player.GetRigidbody().AddForce(Vector2.up * jumpForce);
    }
}
