using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : BaseObject {

    public static GameManager instance;
    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<GameManager>();   
            }
            return instance;
        }
    }

    public Actor Player;

    public override void AwakeObject()
    {
        Player.AwakeObject();
    }

    public override void StartObject()
    {
        Player.StartObject();
    }

}
