using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowLeader : MonoBehaviour
{
    public Transform target;

    public float smoothSpeed = 0.125f;
    public Vector3 offSet;
    private Vector3 velocity = Vector3.zero;

    private void LateUpdate()
    {
        Vector3 desiredPosition = target.position + offSet;
        Vector3 smoothedPosition = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, smoothSpeed);
        transform.position = smoothedPosition;

        transform.LookAt(target);
    }
}
