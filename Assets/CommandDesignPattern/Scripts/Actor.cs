using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actor : BaseObject {

    private Transform _transform;
    private Rigidbody _rigidbody;

    private float _jumpForce;
    private Vector3 _horizontalMovementDistance;
    private Vector3 _verticleMovementDistance;

    public override void AwakeObject()
    {
        _transform = GetComponent<Transform>();
        _rigidbody = GetComponent<Rigidbody>();
    }

    public override void StartObject()
    {
        _jumpForce = 100f;
        _horizontalMovementDistance = new Vector3(1f, 0, 0);
        _verticleMovementDistance = new Vector3(0, 0, 1f);
    }

    public void Jump()
    {
        Debug.Log("<color=red>JUMP!</color>");
        _rigidbody.AddForce(_transform.up * _jumpForce);
    }

    public void MoveRight()
    {
        Debug.Log("<color=blue>MOVE RIGHT!</color>");
        var futurePosition = _transform.position + _horizontalMovementDistance;
        _rigidbody.MovePosition(futurePosition);
    }

    public void MoveLeft()
    {
        Debug.Log("<color=green>MOVE LEFT!</color>");
        var futurePosition = _transform.position - _horizontalMovementDistance;
        _rigidbody.MovePosition(futurePosition);
    }

    internal void MoveDown()
    {
        Debug.Log("<color=yellow>MOVE Down!</color>");
        var futurePosition = _transform.position - _verticleMovementDistance;
        _rigidbody.MovePosition(futurePosition);
    }

    internal void MoveUp()
    {
        Debug.Log("<color=cyan>MOVE Up!</color>");
        var futurePosition = _transform.position + _verticleMovementDistance;
        _rigidbody.MovePosition(futurePosition);
    }
}
