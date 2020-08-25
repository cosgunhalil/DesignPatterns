using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingController
{
    private BuildingModel model;
    private BuildingView view;

    private BuildingController()
    {

    }

    public BuildingController(BuildingModel model, BuildingView view)
    {
        this.model = model;
        this.view = view;

        model.OnNumberOfFloorsChanged += OnNumberOfFloorChanged;
    }

    private void OnNumberOfFloorChanged()
    {
        view.syncNumberOfFloors(model.GetNumberOfFloors());
    }

    public void fetchBuildingFloor(int buildingId)
    {
        model.SetNumberOfFloors(UnityEngine.Random.Range(1, 20));
    }
}
