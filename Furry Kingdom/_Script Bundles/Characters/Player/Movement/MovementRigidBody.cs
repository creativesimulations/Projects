using System;
using System.Collections;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace Furry
{
    public class MovementRigidBody : MonoBehaviour
    {
        public event Action OnJump;

        [SerializeField] private float _jumpForce = 5.5f;
        [SerializeField] private float _rotationSpeed = 5f; // Speed of rotation
        [SerializeField] private float _moveForce = .3f;
        [SerializeField] private float _maxVelocity = 2f;

        [SerializeField] private LayerMask _terrainLayer;

        private bool isTurning;
        private Vector3 _targetDirection; // The direction the player is moving towards
        private Vector3 _newTargetDirection; // The direction the player is moving towards
        private Quaternion _targetRotation;
        private Vector3 _raycastOrigin;
        private PlayerInputHandler _inputHandler;
        private CancellationTokenSource _turnCTS;
        private Rigidbody _rb;
        private bool _freeToMove = true;
        private bool isJumping;

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
        public void Move()
        {
                _rb.AddForce(_moveForce * new Vector3(_inputHandler.MoveInput.x, 0, _inputHandler.MoveInput.y), ForceMode.Force);
        }

        private void GetTurnInput()
        {
            _newTargetDirection = new Vector3(_inputHandler.MoveInput.x, 0, _inputHandler.MoveInput.y).normalized;
            _targetRotation = Quaternion.LookRotation(_newTargetDirection, Vector3.up);

            if ((_targetDirection != _newTargetDirection) && !isTurning)
            {
                Turn(_turnCTS.Token);
            }
        }

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
        private void RBJump()
        {
            if (CanJump())
            {
                _rb.AddForce(Vector3.up * _jumpForce, ForceMode.VelocityChange);
                OnJump?.Invoke();
            }
        }
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

        public void ResetLocation(Vector3 location)
        {
            _rb.velocity = Vector3.zero;
            transform.position = location;
        }
        public void StopMovement()
        {
            _freeToMove = false;
        }
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