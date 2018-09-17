using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackDecorator : Attack
{
    protected Attack decoratedAttack;
    public AttackDecorator(Attack decoreatedAttack)
    {
        this.decoratedAttack = decoreatedAttack;
    }

    public override void SetDamage()
    {
        this.decoratedAttack.SetDamage();
    }
}
