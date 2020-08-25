using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingModel
{
    public delegate void NumberOfFloorsChangedAction();
    public event NumberOfFloorsChangedAction OnNumberOfFloorsChanged;

    private int numberOfFloors;

    public void SetNumberOfFloors(int numberOfFloors)
    {
        this.numberOfFloors = numberOfFloors;

        OnNumberOfFloorsChanged?.Invoke();//notify listeners
    }

    public int GetNumberOfFloors()
    {
        return numberOfFloors;
    }

}
