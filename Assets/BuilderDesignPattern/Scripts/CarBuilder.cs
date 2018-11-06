using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarBuilder : VehicleBuilder
{
    public CarBuilder()
    {
        vehicle = new Vehicle("Car");
    }

    public override void BuildEngine()
    {
        vehicle.MaxEngineRPM = 7500f;
    }

    public override void BuildGearbox()
    {
        vehicle.MaxSpeedsPerGear = new float[]
        {
            35f,
            70f,
            85f,
            100f,
            120f,
            150f
        };
    }

    public override void BuildShape()
    {
        vehicle.Shape = "Sedan";
    }

    public override void BuildWheels()
    {
        vehicle.WheelCount = 4;
    }
}
