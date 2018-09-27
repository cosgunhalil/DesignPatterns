using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObserverDesignPatternDemo : MonoBehaviour {

    private ScoreDataSubject subject = new ScoreDataSubject();

	void Start () 
    {
        var superScorer = new SuperScorer(subject);
        var babySteps = new BabySteps(subject);
        var immortalPlayer = new ImmortalPlayer(subject);

        StartCoroutine("IncreaseScore");

	}

    private IEnumerator IncreaseScore()
    {
        var wait = new WaitForSeconds(.5f);
        var score = 0;
        while (true)
        {
            yield return wait;
            subject.SetScore(score++);
        }
    }

}
