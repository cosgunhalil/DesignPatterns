﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {
    
    private Transform _transform;
    private float _speed;
    private Vector2 _movementDirection;
    private CameraLimits _movementLimit;

    public void Init()
    {
        _speed = 10f;
        _transform = GetComponent<Transform>();
        _movementLimit = CameraManager.Instance.GetCameraLimits();
    }

    public void SetMovementDirection(Vector2 movementDirection)
    {
        _movementDirection = movementDirection;
    }

    public void SetPosition(Vector2 position)
    {
        _transform.position = position;
    }

    public void Move()
    {
        if (CanMove())
        {
            _transform.Translate(_movementDirection * _speed * Time.deltaTime);
        }
        else
        {
            BulletEventManager.Instance.BulletDeleted(this);
        }
    }

    private bool CanMove()
    {
        bool canMove = false;
        if (_transform.position.x < _movementLimit.xMax && 
            _transform.position.x > _movementLimit.xMin &&
            _transform.position.y < _movementLimit.yMax &&
            _transform.position.y > _movementLimit.yMin)
        {
            canMove = true;
        }

        return canMove;
    }


    public void ActivateBullet(bool isActive)
    {
        gameObject.SetActive(isActive);
    }
}
