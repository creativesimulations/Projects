using MilkShake.Demo;
using System.Collections;
using UnityEngine;

namespace EpicBall
{
    public class CylinderMovement : Chaser
    {
        [Header("Movement settings.")]
        [Tooltip("Movement speed while chasing the player.")]
        [SerializeField] private float _desiredSpeed = 30;
        [Tooltip("Maximum drag on the block. This mainly affects the acceleration and stopping speeds.")]
        [SerializeField] private float _maximumDrag = .5f;
        [Tooltip("The amount of force applied to the block while chasing the player.")]
        [SerializeField] private float _forceConstant = 1000;

        private bool _isHorizontal;
        private Block _block;
        private bool isGiant;
        private float _forceMultiplier;
        private Vector3 _right;
        private Vector3 _up;
        private Vector3 _forward;
        private Vector3 _toTarget;
        private float _actualAngleZ;
        private float _actualAngleX;

        private void Awake()
        {
            _block = GetComponent<Block>();
            if (GetComponent<GiantBlock>() != null )
            {
                isGiant = true;
            }
        }

        /// <summary>
        /// Applies movement force to the rigid body of the object.
        /// </summary>
        /// <param name="direction"></param> The direction the object should be moved.
        public void Roll(Vector3 direction)
        {
            _block.Rb.drag = Mathf.Lerp(_maximumDrag, 0, direction.magnitude);
            _forceMultiplier = Mathf.Clamp01((_desiredSpeed - _block.Rb.velocity.magnitude) / _desiredSpeed);
            _block.Rb.AddForce(direction * (_forceMultiplier * Time.deltaTime * _forceConstant));
        }

        /// <summary>
        /// While active, the object will chase the player on the correct side of the cylinder.
        /// </summary>
        /// <param name="player"></param> The player game object to chase.
        /// <returns></returns>
        public IEnumerator LocateSide(GameObject player)
        {
            active = true;
            InvokeRepeating("CheckHorizontal", 0f, 1f);
            while (active == true)
            {
                _right = transform.up;
                _up = Vector3.up;
                _forward = Vector3.Cross(_right, _up);
                _toTarget = (player.transform.position - transform.position).normalized;

                if ((Physics.Raycast(transform.position, -Vector3.up, transform.localScale.z)))
                {
                    if (_isHorizontal)
                    {
                        if (Vector3.Dot(_toTarget, _forward) > 0)
                        {
                            Roll(_forward);
                        }
                        else
                        {
                            Roll(-_forward);
                        }
                    }
                }
                yield return new WaitForEndOfFrame();
            }
        }

        /// <summary>
        /// Checks if the cylinder is mostly horizontal. This is used to prevent the cylinder from chasing the player if it isn't mostly horizontal.
        /// </summary>
        public void CheckHorizontal()
        {
            _actualAngleZ = Repeat(transform.rotation.eulerAngles.z, 360);
            _actualAngleX = Repeat(transform.rotation.eulerAngles.x, 360);
            if (((_actualAngleZ < 93 && _actualAngleZ > 87) || (_actualAngleX < 93 && _actualAngleX > 87)) || ((_actualAngleZ < 273 && _actualAngleZ > 267) || (_actualAngleX < 273 && _actualAngleX > 267)))
            {
                _isHorizontal = true;
            }
            else
            {
                _isHorizontal = false;
            }
        }

        /// <summary>
        /// Returns the angle at which the object (Cylinder) is tilted.
        /// </summary>
        /// <param name="t"></param> The axis angle to check.
        /// <param name="length"></param> the amount of angles (360) to check against.
        /// <returns></returns>
        public float Repeat(float t, float length)
        {
            return t - Mathf.Floor(t / length) * length;
        }

        /// <summary>
        /// If the cylinder isn't active it will begin chasing the player game object.
        /// </summary>
        /// <param name="player"></param> The player game object to chase.
        public override void Chase(GameObject player)
        {
            if (!active)
            {
                StartCoroutine(LocateSide(player));
            }
        }

        /// <summary>
        /// Deactivates the cylinder block through the base method and stops all coroutines on this block.
        /// </summary>
        public override void StopChasing()
        {
            if (!isGiant)
            {
                base.StopChasing();
                StopAllCoroutines();
                CancelInvoke("CheckHorizontal");
            }
        }
    }
}