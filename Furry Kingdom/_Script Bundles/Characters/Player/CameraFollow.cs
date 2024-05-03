using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Follow parameters")]
    [Tooltip("Object To Follow")]
    [SerializeField] Transform _objectToFollow;
    [Tooltip("Follow speed")]
    [SerializeField, Range(.01f, 1f)] private float _smoothSpeed;
    [Tooltip("Offset angle")]
    [SerializeField] private Vector3 _offset;

    private Vector3 _velocity = Vector3.zero;

    private void Update()
    {
        Follow();
    }

    /// <summary>
    /// Set new transform position according to target position.
    /// </summary>
    private void Follow()
    {
        Vector3 desiredPosition = _objectToFollow.position + _offset;
        transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref _velocity, _smoothSpeed);
    }
}
