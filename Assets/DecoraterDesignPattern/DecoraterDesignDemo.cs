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

        Attack poisonedFirePunch = new PoisonAttackDecorator(new FireAttackDecorator(punch));

        poisonPunch.SetDamage();
        Debug.Log("<color=green>*********</color>");
        poisonHighKick.SetDamage();
        Debug.Log("<color=green>*********</color>");
        poisonedFirePunch.SetDamage();
    }
}
