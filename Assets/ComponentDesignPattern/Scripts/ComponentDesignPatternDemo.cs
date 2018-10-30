using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComponentDesignPatternDemo : MonoBehaviour {

    public Player[] Players;

	void Start ()
    {
        for (int i = 0; i < Players.Length; i++)
        {
            Players[i].PlayerStart();
        }
    }

	void Update ()
    {
        for (int i = 0; i < Players.Length; i++)
        {
            Players[i].PlayerUpdate();
        }
	}
}
