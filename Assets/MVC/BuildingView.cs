using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingView : MonoBehaviour
{
    public void syncNumberOfFloors(int numberOfFloors)
    {
        Debug.LogFormat("{0} floor(s) constructed!", numberOfFloors);
    }
}
