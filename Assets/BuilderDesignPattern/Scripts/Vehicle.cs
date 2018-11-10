using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vehicle {

    private string vehicleType;

    public Vehicle(string vehicleType)
    {
        this.vehicleType = vehicleType;
    }

    public string Shape;
    public float MaxEngineRPM;
    public float[] MaxSpeedsPerGear;
    public int WheelCount;

    public void PrintParts()
    {
        Debug.Log("Vehicle Type = " + this.vehicleType);
        Debug.Log("Shape = " + this.Shape);
        Debug.Log("Max Engine RPM = " + this.MaxEngineRPM);
        for (int i = 0; i < MaxSpeedsPerGear.Length; i++)
        {
            Debug.Log("Gear " + i + " speed is " + MaxSpeedsPerGear[i]);
        }

        Debug.Log("Wheel Count = " + WheelCount);
    }

}
