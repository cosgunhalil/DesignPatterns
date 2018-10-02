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
            if (subject.GetScore() > target)
            {
                Achieve();
                subject.Detach(this);
            }
        }

        protected virtual void Achieve()
        {
        
        }
    }
