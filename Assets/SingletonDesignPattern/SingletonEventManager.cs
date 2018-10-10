using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingletonEventManager : Singleton<SingletonEventManager> {

    public void SendEvent()
    {
        Debug.Log("Event sended!");
    }

}
