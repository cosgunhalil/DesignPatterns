using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImmortalPlayer : Achievement
{
    public ImmortalPlayer(ScoreDataSubject subject) : base(subject)
    {
        target = 60;
    }

    protected override void Achieve()
    {
        Debug.Log("Immortal Player Achieved!!!");
    }
}
