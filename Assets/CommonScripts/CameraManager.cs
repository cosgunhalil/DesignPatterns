using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour {

    private static CameraManager instance;
    public static CameraManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = (CameraManager)FindObjectOfType(typeof(CameraManager));
                instance.Init();
            }

            return instance;
        }
    }

    private Camera _mainCamera;
    private Vector2 _screenSize;
    private Transform _transform;

    private void Init()
    {
        _mainCamera = Camera.main;
        _transform = GetComponent<Transform>();
        CalculateScreenSize();
    }

    private void CalculateScreenSize()
    {
        float height = 2f * _mainCamera.orthographicSize;
        float width = height * _mainCamera.aspect;

        _screenSize = new Vector2(width, height);
    }

    public Vector2 GetScreenSize()
    {
        return _screenSize;
    }

    public Vector2 GetPosition()
    {
        return _transform.position;
    }
}
