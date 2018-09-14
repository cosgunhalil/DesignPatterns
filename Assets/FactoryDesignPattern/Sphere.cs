using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sphere : Shape
{
    private float _radious;
    
    public Sphere(Vector3 center, float radius)
    {
        _center = center;
        _radious = radius;
    }

    public override void Draw()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawSphere(_center, _radious);
    }
}
