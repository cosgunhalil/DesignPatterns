using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BabySteps : Achievement {
    
    public BabySteps(ScoreDataSubject subject) : base(subject)
    {
        target = 1;
    }

    protected override void Achieve()
    {
        Debug.Log("Baby Steps Achieved!!!");
    }

}
