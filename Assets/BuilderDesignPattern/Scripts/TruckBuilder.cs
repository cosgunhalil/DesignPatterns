using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TruckBuilder : VehicleBuilder
{
    public TruckBuilder()
    {
        vehicle = new Vehicle("Truck");
    }

    public override void BuildEngine()
    {
        vehicle.MaxEngineRPM = 16000f;
    }

    public override void BuildGearbox()
    {
        vehicle.MaxSpeedsPerGear = new float[]
        {
            120f,
            200f,
            300f,
            450f,
            60f,
            850f,
            1600f,
            8000f,
            16000f
        };
    }

    public override void BuildShape()
    {
        vehicle.Shape = "Muscle Truck";
    }

    public override void BuildWheels()
    {
        vehicle.WheelCount = 12;
    }
}
