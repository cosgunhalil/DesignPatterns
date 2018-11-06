using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuilderPatternDemo : MonoBehaviour {

	void Start () {

        Director director = new Director();

        var carBuilder = new CarBuilder();
        var bicycleBuilder = new BicycleBuilder();

        director.Construct(carBuilder);
        director.Construct(bicycleBuilder);

        var car = carBuilder.GetVehicle();
        var bicycle = bicycleBuilder.GetVehicle();

        Debug.Log("<color=red>Car Parts</color>");
        car.PrintParts();
        Debug.Log("<color=blue>Bicycle Parts</color>");
        bicycle.PrintParts();

	}
}
