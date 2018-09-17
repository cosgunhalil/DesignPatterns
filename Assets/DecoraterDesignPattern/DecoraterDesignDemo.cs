using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecoraterDesignDemo : MonoBehaviour {

    private void Start()
    {
        Attack punch = new Punch();
        Attack poisonPunch = new PoisonAttackDecorator(punch);

        poisonPunch.SetDamage();
    }
}
