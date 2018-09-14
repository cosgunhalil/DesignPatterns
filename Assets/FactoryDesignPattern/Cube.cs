using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube : Shape
{
    private Vector3 _size;
    
    public Cube(Vector3 center, Vector3 size)
    {
        _center = center;
        _size = size;
    }
    
    public override void Draw()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawCube(_center, _size);
    }
}
