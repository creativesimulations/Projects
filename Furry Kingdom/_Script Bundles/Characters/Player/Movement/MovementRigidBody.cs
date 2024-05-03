using System;
using System.Collections;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace Furry
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(CapsuleCollider))]
    public class MovementRigidBody : MonoBehaviour
    {
        /// <summary>
        /// This is invoked when the player jumps.
        /// </summary>
        public event Action OnJump;

        [Header("Movement field")]
        [Tooltip("Amount of force used to jump.")]
        [SerializeField] private float _jumpForce = 5.5f;
        [Tooltip("Turn speed.")]
        [SerializeField] private float _rotationSpeed = 5f;
        [Tooltip("Amount of force used to move.")]
        [SerializeField] private float _moveForce = .3f;

        [Tooltip("Layer name for the walkable terrain of the player.")]
        [SerializeField] private LayerMask _terrainLayer;

        private bool isTurning;
        private Vector3 _targetDirection;
        private Vector3 _newTargetDirection;
        private Quaternion _targetRotation;
        private Vector3 _raycastOrigin;
        private PlayerInputHandler _inputHandler;
        private CancellationTokenSource _turnCTS;
        private Rigidbody _rb;
        private bool _freeToMove = true;

        protected void Awake()
        {
            _rb = GetComponent<Rigidbody>();
            _turnCTS = new CancellationTokenSource();
        }

        private void Start()
        {
            _inputHandler = GetComponent<PlayerInputHandler>();
        }

        private void FixedUpdate()
        {
            if (_freeToMove)
            {
                Move();
                GetTurnInput();
                if (_inputHandler.JumpTriggered)
                {
                    RBJump();
                }
            }
        }
        /// <summary>
        /// Apply force to the player for movement.
        /// </summary>
        public void Move()
        {
            _rb.AddForce(_moveForce * new Vector3(_inputHandler.MoveInput.x, 0, _inputHandler.MoveInput.y), ForceMode.Force);
        }

        /// <summary>
        /// Check if there is new turn input. If so, the async method Turn will run.
        /// </summary>
        private void GetTurnInput()
        {
            _newTargetDirection = new Vector3(_inputHandler.MoveInput.x, 0, _inputHandler.MoveInput.y).normalized;
            _targetRotation = Quaternion.LookRotation(_newTargetDirection, Vector3.up);

            if ((_targetDirection != _newTargetDirection) && !isTurning)
            {
                Turn(_turnCTS.Token);
            }
        }

        /// <summary>
        /// Turns the player while it hasn't yet reached the new turn rotation and isn't canceled.
        /// </summary>
        /// <param name="ct"></param> Cancellation token
        private async void Turn(CancellationToken ct)
        {
            isTurning = true;
            while (_inputHandler.MoveTriggered)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, _targetRotation, _rotationSpeed * Time.deltaTime);

                await Task.Yield();
            }
            _targetDirection = transform.forward;
            isTurning = false;
        }

        /// <summary>
        /// Apply force for the player to jump.
        /// </summary>
        private void RBJump()
        {
            if (CanJump())
            {
                _rb.AddForce(Vector3.up * _jumpForce, ForceMode.VelocityChange);
                OnJump?.Invoke();
            }
        }

        /// <summary>
        /// Returns true if the player is grounded.
        /// </summary>
        /// <returns></returns>
        private bool CanJump()
        {
            RaycastHit _hit;
            _raycastOrigin = transform.position + Vector3.up * 0.1f;
            if (Physics.Raycast(_raycastOrigin, Vector3.down, out _hit, .2f, _terrainLayer))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Stops movement of the player and resets its location.
        /// </summary>
        /// <param name="location"></param> The location the player will be reset to.
        public void ResetLocation(Vector3 location)
        {
            _rb.velocity = Vector3.zero;
            transform.position = location;
        }

        /// <summary>
        /// Prevents the player from moving.
        /// </summary>
        public void PauseMovement()
        {
            _freeToMove = false;
        }

        /// <summary>
        /// Allows the player to continue moving.
        /// </summary>
        public void ResumeMovement()
        {
            _freeToMove = true;
        }

        private void OnDisable()
        {
            _inputHandler.OnMove -= GetTurnInput;
            _inputHandler.OnRBJump -= RBJump;
            _turnCTS?.Cancel();
        }
    }

}