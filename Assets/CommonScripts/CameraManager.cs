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
    [SerializeField]
    private CameraLimits _cameraLimits;

    private void Init()
    {
        _mainCamera = Camera.main;
        _transform = GetComponent<Transform>();
        CalculateScreenSize();
        CalculateScreenLimits();
    }

    private void CalculateScreenLimits()
    {
        var cameraTransform = GetComponent<Transform>();
        var xLength = _screenSize.x / 2f;
        var yLength = _screenSize.y / 2f;

        _cameraLimits = new CameraLimits
        {
            xMax = cameraTransform.position.x + xLength,
            xMin = cameraTransform.position.x - xLength,
            yMax = cameraTransform.position.y + yLength,
            yMin = cameraTransform.position.y - yLength
        };
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

    public CameraLimits GetCameraLimits()
    {
        return _cameraLimits;
    }
}

[Serializable]
public struct CameraLimits
{
    public float xMax;
    public float xMin;
    public float yMax;
    public float yMin;
}
