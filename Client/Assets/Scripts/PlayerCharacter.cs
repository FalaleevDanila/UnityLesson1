using System;
using UnityEngine;
using System.Collections.Generic;
using Colyseus.Schema;

public class PlayerCharacter : Character
{
    [SerializeField] private Health _health;
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private Transform _head;
    [SerializeField] private Transform _cameraPoint;
    [SerializeField] private float _maxHeadAngle = 90;
    [SerializeField] private float _minHeadAngle = -90;
    [SerializeField] private float _jumpForce = 5F;
    [SerializeField] private CheckFly _checkFly;
    [SerializeField] private float _jumpDelay = 0.2F;

    private float _inputH;
    private float _inputV;
    private float _rotateY;
    private float _currentRotateX;
    private float _jumpTime;

    private void Start()
    {
        Transform camera = Camera.main.transform;
        camera.parent = _cameraPoint;
        camera.localPosition = Vector3.zero;
        camera.localRotation = Quaternion.identity;
        _health.SetMax(maxHealth);
        _health.SetCurrent(maxHealth);
    }

    public void SetInput(float h, float v, float rotateY)
    {
        _inputH = h;
        _inputV = v;
        _rotateY += rotateY;
    }

    private void RotateY()
    {
        _rigidbody.angularVelocity = new Vector3(0, _rotateY, 0);
        _rotateY = 0;
    }



    public void RotateX(float value)
    {
        _currentRotateX = Math.Clamp(_currentRotateX + value, _minHeadAngle, _maxHeadAngle);
        _head.localEulerAngles = new Vector3(_currentRotateX, 0, 0);
    }

    void FixedUpdate()
    {
        Move();
        RotateY();
    }

    void Move()
    {
        Vector3 velocity = (transform.forward * _inputV + transform.right * _inputH).normalized * speed;
        velocity.y = _rigidbody.linearVelocity.y;
        base.velocity = velocity;
        _rigidbody.linearVelocity = base.velocity;
    }

    public void GetMoveInfo(out Vector3 position, out Vector3 velocity, out float rotateX, out float rotateY)
    {
        position = transform.position;
        velocity = _rigidbody.linearVelocity;

        rotateX = _head.localEulerAngles.x;
        rotateY = transform.eulerAngles.y;

    }

    public void Jump()
    {
        if (_checkFly.IsFly) return;
        if (Time.time - _jumpTime < _jumpDelay) return;
        _jumpTime = Time.time;
        _rigidbody.AddForce(0, _jumpForce, 0, ForceMode.VelocityChange);

    }

    internal void OnChange(List<DataChange> changes)
    {
        foreach (var dataChange in changes)
        {
            switch (dataChange.Field)
            {
                case "currentHP":
                    _health.SetCurrent((sbyte)dataChange.Value);
                    break;
                
                default:
                    //Debug.LogWarning("Dont know this field: " + dataChange.Field);
                    break;
            }
        } 
    }
}
