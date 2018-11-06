using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Director : MonoBehaviour {

    public void Construct(VehicleBuilder vehicleBuilder)
    {
        vehicleBuilder.BuildShape();    
        vehicleBuilder.BuildEngine();
        vehicleBuilder.BuildGearbox();
        vehicleBuilder.BuildWheels();
    }
}
