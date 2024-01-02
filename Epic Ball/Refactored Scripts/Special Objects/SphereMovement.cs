using System.Collections;
using UnityEngine;

namespace EpicBall
{
    public class SphereMovement : Chaser
    {
        [SerializeField] private float _desiredSpeed = 30f;
        [SerializeField] private float _maximumDrag = .5f;
        [SerializeField] private float _forceConstant = 10000f;
        private Block _block;
        private Vector3 _direction;
        private float _forceMultiplier;

        private void Awake()
        {
            _block = GetComponent<Block>();
        }

        /// <summary>
        /// Checks if the sphere isn't already chasing and begins the chase coroutine.
        /// </summary>
        /// <param name="player"></param> The player object.
        public override void Chase(GameObject player)
        {
            if (!active)
            {
                StartCoroutine(ChasePlayer(player));
            }
        }

        /// <summary>
        /// Continues to update the sphere's movement according to where the player object is until the spehere is deactivated.
        /// </summary>
        /// <param name="player"></param> The player game object to chase.
        /// <returns></returns>
        private IEnumerator ChasePlayer(GameObject player)
        {
            active = true;
            while (active == true)
            {
                if ((Physics.Raycast(transform.position, -Vector3.up, transform.localScale.z)))
                {
                    _direction = player.transform.position - transform.position;
                    _direction = _direction.normalized;
                    Move(_direction);
                }
                yield return new WaitForEndOfFrame();
            }
        }

        /// <summary>
        /// Calculates the direction and force at which the spehere should move and applies it to the rigid body.
        /// </summary>
        /// <param name="moveDirection"></param> The direction the block should be moved.
        private void Move(Vector3 moveDirection)
        {
            _block.Rb.drag = Mathf.Lerp(_maximumDrag, 0, moveDirection.magnitude);
            _forceMultiplier = Mathf.Clamp01((_desiredSpeed - _block.Rb.velocity.magnitude) / _desiredSpeed);
            _block.Rb.AddForce(moveDirection * (_forceMultiplier * Time.deltaTime * _forceConstant));
        }

        /// <summary>
        /// Runs the base (Chaser) script to deactivate the movement and stops all running coroutines on this object.
        /// </summary>
        public override void StopChasing()
        {
            base.StopChasing();
            StopAllCoroutines();
        }
    }
}