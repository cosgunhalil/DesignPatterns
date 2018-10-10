using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingletonAdManager : Singleton<SingletonAdManager> {

    public void ShowSingletonAd()
    {
        Debug.Log("Singleton Ad Shown");
    }
	
}
