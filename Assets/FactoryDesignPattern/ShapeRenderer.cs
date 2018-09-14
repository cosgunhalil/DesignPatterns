using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapeRenderer : MonoBehaviour {

    private List<Shape> Shapes;

	// Use this for initialization
	void Start () 
    {
        CreateShapes();
	}

    private void CreateShapes()
    {
        ShapeFactory shapeFactory = new ShapeFactory();
        Shapes = new List<Shape>
        {
            shapeFactory.GetShape(ShapeType.cube),
            shapeFactory.GetShape(ShapeType.sphere)
        };
    }

    private void OnDrawGizmos()
    {
        RenderShapes();
    }

    private void RenderShapes()
    {
        for (int i = 0; i < Shapes.Count; i++)
        {
            Shapes[i].Draw();
        }
    }

}
