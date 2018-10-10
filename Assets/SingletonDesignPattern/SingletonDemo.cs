using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingletonDemo : MonoBehaviour {

	void Start () {
        AdManager.Instance.ShowAd();
	}
}
