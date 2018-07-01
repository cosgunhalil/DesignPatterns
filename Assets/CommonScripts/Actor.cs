using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actor : BaseObject {

    private Transform _transform;
    private Rigidbody2D _rigidbody;

    private float _jumpForce;
    private Vector2 _moveDistance;

    public override void AwakeObject()
    {
        _transform = GetComponent<Transform>();
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    public override void StartObject()
    {
        _jumpForce = 100f;
        _moveDistance = new Vector2(1f, 0);
    }

    public void Jump()
    {
        Debug.Log("<color=red>JUMP!</color>");
        _rigidbody.AddForce(_transform.up * _jumpForce);
    }

    public void MoveRight()
    {
        Debug.Log("<color=blue>MOVE RIGHT!</color>");
        var futurePosition = (Vector2)_transform.position + _moveDistance;
        _rigidbody.MovePosition(futurePosition);
    }

    public void MoveLeft()
    {
        Debug.Log("<color=green>MOVE LEFT!</color>");
        var futurePosition = (Vector2)_transform.position - _moveDistance;
        _rigidbody.MovePosition(futurePosition);
    }
}
