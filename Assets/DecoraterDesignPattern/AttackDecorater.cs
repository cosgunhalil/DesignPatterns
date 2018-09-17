using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AttackDecorater : Attack
{
    protected Attack decoratedAttack;
    public AttackDecorater(Attack decoreatedAttack)
    {
        this.decoratedAttack = decoreatedAttack;
    }

    public override void SetDamage()
    {
        this.decoratedAttack.SetDamage();
    }
}
