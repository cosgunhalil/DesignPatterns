using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class VehicleBuilder {

    protected Vehicle vehicle;

    public Vehicle GetVehicle()
    {
        return vehicle;
    }

    public abstract void BuildShape();
    public abstract void BuildEngine();
    public abstract void BuildGearbox();
    public abstract void BuildWheels();
}
