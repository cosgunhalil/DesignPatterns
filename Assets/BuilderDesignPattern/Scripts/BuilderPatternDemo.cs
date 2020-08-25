using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuilderPatternDemo : MonoBehaviour {

	void Start () {

        Shop director = new Shop();

        var carBuilder = new CarBuilder();
        var truckBuilder = new TruckBuilder();

        director.Construct(carBuilder);
        director.Construct(truckBuilder);

        var car = carBuilder.GetVehicle();
        var bicycle = truckBuilder.GetVehicle();

        Debug.Log("<color=red>Car Parts</color>");
        car.PrintParts();
        Debug.Log("<color=blue>Truck Parts</color>");
        bicycle.PrintParts();

	}
}
