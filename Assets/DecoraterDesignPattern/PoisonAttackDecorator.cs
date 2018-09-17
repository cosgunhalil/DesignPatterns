using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonAttackDecorator : AttackDecorater
{
    public PoisonAttackDecorator(Attack decoreatedAttack) : base(decoreatedAttack)
    {
    }

    public override void SetDamage()
    {
        decoratedAttack.SetDamage();
        SetPoison();
    }

    private void SetPoison()
    {
        Debug.Log("Poison Setted!");
    }
}
