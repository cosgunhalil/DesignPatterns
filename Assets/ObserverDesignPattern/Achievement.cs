using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Achievement : ScoreDataObserver
{
    protected float target;

    public Achievement(ScoreDataSubject subject) : base(subject)
    {
    }

    public override void OnNotify()
    {
        if (model.GetScore() > target)
        {
            Achieve();
            model.Detach(this);
        }
    }

    protected virtual void Achieve()
    {
        
    }
}
