using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//http://gameprogrammingpatterns.com/
//https://sourcemaking.com/design_patterns/observer/cpp/3

//subject - we are observing changes on this object
public class ScoreDataSubject {
    
    private int score;
    private List<ScoreDataObserver> _observers = new List<ScoreDataObserver>();

    public void Attach(ScoreDataObserver observer)
    {
        _observers.Add(observer);
    }

    public void Detach(ScoreDataObserver observer)
    {
        _observers.Remove(observer);
    }

    public void Notify()
    {
        for (int i = 0; i < _observers.Count; i++)
        {
            _observers[i].OnNotify();
        }
    }

    public int GetScore()
    {
        return score;
    }

    public void SetScore(int score)
    {
        this.score = score;
        Notify();
    }

}
