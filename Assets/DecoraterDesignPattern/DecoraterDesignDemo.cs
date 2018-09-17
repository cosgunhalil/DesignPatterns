using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecoraterDesignDemo : MonoBehaviour {

    private void Start()
    {
        Attack punch = new Punch();
        Attack poisonPunch = new PoisonAttackDecorator(punch);

        Attack highKick = new HighKick();
        Attack poisonHighKick = new PoisonAttackDecorator(highKick);

        poisonPunch.SetDamage();
        poisonHighKick.SetDamage();
    }
}
