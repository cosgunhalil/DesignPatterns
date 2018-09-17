using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Punch : Attack
{
    public override void SetDamage()
    {
        Debug.Log("Punch!");
    }
}
