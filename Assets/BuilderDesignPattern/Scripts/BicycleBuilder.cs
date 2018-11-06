using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BicycleBuilder : VehicleBuilder
{
    public BicycleBuilder()
    {
        vehicle = new Vehicle("Bicycle");
    }

    public override void BuildEngine()
    {
        vehicle.MaxEngineRPM = 2000f;
    }

    public override void BuildGearbox()
    {
        vehicle.MaxSpeedsPerGear = new float[]
        {
            12f,
            20f,
            30f,
            45f,
            60f
        };
    }

    public override void BuildShape()
    {
        vehicle.Shape = "Mountain Bike Shape";
    }

    public override void BuildWheels()
    {
        vehicle.WheelCount = 2;
    }
}
