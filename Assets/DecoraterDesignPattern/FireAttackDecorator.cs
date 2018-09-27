using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireAttackDecorator : AttackDecorator {

    public FireAttackDecorator(Attack decoreatedAttack) : base(decoreatedAttack)
    {
    }

    public override void SetDamage()
    {
        decoratedAttack.SetDamage();
        SetFire();
    }

    private void SetFire()
    {
        Debug.Log("Fire Setted");
    }
}
