using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunAbility : PhysicsComponent
{
    private float maxSpeed;
    private float currentSpeed;
    private float acceleration;

    public RunAbility(Player player, float maxSpeed, float acceleration)
    {
        this.player = player;
        this.maxSpeed = maxSpeed;
        currentSpeed = 0f;
        this.acceleration = acceleration;
    }

    public override void ComponentUpdate()
    {
        var playerRigidbody = player.GetRigidbody();
        if (currentSpeed <= maxSpeed)
        {
            currentSpeed += acceleration;
            
        }

        playerRigidbody.velocity = new Vector2(currentSpeed, playerRigidbody.velocity.y);

    }
}
