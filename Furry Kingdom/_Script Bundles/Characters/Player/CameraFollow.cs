using Furry;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Follow parameters")]
    [Tooltip("Object To Follow")]
    [SerializeField] Transform _objectToFollow;
    [SerializeField, Range(.01f, 1f), Tooltip("Follow speed")]
    private float _smoothSpeed;
    [SerializeField, Tooltip("Follow speed")]
    private Vector3 _offset;

    private Vector3 _velocity = Vector3.zero;

    private void Update()
    {
        Follow();
    }
    private void Follow()
    {
        Vector3 desiredPosition = _objectToFollow.position + _offset;
        transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref _velocity, _smoothSpeed);
    }
}
