using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapeFactory {

    public Shape GetShape(ShapeType shapeType)
    {
        switch (shapeType)
        {
            case ShapeType.cube:
                return new Cube(new Vector3(10,10,10), new Vector3(50,70,90));
            case ShapeType.sphere:
                return new Sphere(new Vector3(80,10,10), 30f);
            default:
                return null;
        }
    }
}
