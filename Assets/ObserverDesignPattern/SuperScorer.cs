using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuperScorer : Achievement
{
    public SuperScorer(ScoreDataSubject subject) : base(subject)
    {
        target = 19;
    }

    protected override void Achieve()
    {
        Debug.Log("Super Scorer Achieved!!!");
    }
}
