using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdManager : SingletonMonoBehaviour<AdManager> {

    private AdManager()
    {

    }

    static AdManager()
    {

    }


    public void ShowAd()
    {
        Debug.Log("Ad is shown!");
        SingletonEventManager.Instance.SendEvent();
        SingletonAdManager.Instance.ShowSingletonAd();
    }
}
