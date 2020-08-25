using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MVCDemo : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var buildingModel = new BuildingModel();//model is created
        var buildingView = new GameObject("buildingView").AddComponent<BuildingView>(); //view is created

        var buildingController = new BuildingController(buildingModel, buildingView); // controller is created

        var buildingId = 377;
        buildingController.fetchBuildingFloor(buildingId);

    }
}
