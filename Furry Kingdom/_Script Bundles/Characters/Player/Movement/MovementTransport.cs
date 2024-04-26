/*
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace Furry
{
    [RequireComponent(typeof(PlayerInputHandler))]
    public class MovementTransport : MonoBehaviour
    {
        [SerializeField, Range(1, 20)] private float _moveSpeed = 5;
        [SerializeField, Range(1, 20)] private float _jumpHeight = 6.5f;
        [SerializeField] private bool _variableJump;
        [SerializeField] private float _rotationSpeed = 5f; // Speed of rotation

        private bool _isJumping;
        private bool isTurning;
        private float _velocity;
        private float _gravityScale = 5;
        private RaycastHit hit;

        private Vector3 _targetDirection; // The direction the player is moving towards
        private Vector3 _newTargetDirection; // The direction the player is moving towards
        private Quaternion _targetRotation;

        private PlayerInputHandler _inputHandler;
        private CancellationTokenSource _turnCTS;

        private void Awake()
        {
            _turnCTS = new CancellationTokenSource();
            _inputHandler = GetComponent<PlayerInputHandler>();
        }

        private void Start()
        {
            _inputHandler.OnJumpHeld += Jump;
            if (_variableJump)
            {
                _inputHandler.OnJumpRelease += StopJump;
            }
            _inputHandler.OnMove += GetTurnInput;
        }

        private void FixedUpdate()
        {
            Move();
        }
        public void Move()
        {
            transform.position += _moveSpeed * Time.deltaTime * new Vector3(_inputHandler.MoveInput.x, 0, _inputHandler.MoveInput.y);
        }

        private void GetTurnInput()
        {
            _targetDirection = new Vector3(_inputHandler.MoveInput.x, 0, _inputHandler.MoveInput.y).normalized;
            _targetRotation = Quaternion.LookRotation(_targetDirection, Vector3.up);

            if ((_targetDirection != _newTargetDirection) && !isTurning)
            {
                Turn(_turnCTS.Token);
            }
        }

        private async void Turn(CancellationToken ct)
        {
            _newTargetDirection = _targetDirection;
            isTurning = true;
            while (_inputHandler.MoveTriggered)
            {
                // Interpolate between the current rotation and the target rotation
                transform.rotation = Quaternion.Slerp(transform.rotation, _targetRotation, _rotationSpeed * Time.deltaTime);

                await Task.Yield();
            }
            isTurning = false;
            _turnCTS?.Cancel();
        }

        public void CheckHeight()
        {
            Ray ray = new Ray(transform.position, -Vector3.up);
            float halfSize = gameObject.transform.lossyScale.y / 4;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.distance < halfSize)
                {
                    _isJumping = false;
                    _velocity = 0;
                    transform.position = new Vector3(transform.position.x, (hit.point.y + halfSize), transform.position.z);
                }
            }
        }
        public void Jump()
        {
            if (!_isJumping)
            {
                _velocity = Mathf.Sqrt(_jumpHeight * -2 * (Physics.gravity.y * _gravityScale));
                _isJumping = true;
                ApplyGravity();
            }
        }

        private void StopJump()
        {
            if (_isJumping && _velocity > 0)
            {
                _velocity = 0;
            }
        }

        private async void ApplyGravity()
        {
            while (_isJumping)
            {
                _velocity += Physics.gravity.y * _gravityScale * Time.deltaTime;
                CheckHeight();
                transform.Translate(new Vector3(0, _velocity, 0) * Time.deltaTime);
                await Task.Yield();
            }
        }

        private void OnDisable()
        {
            _inputHandler.OnJumpHeld -= Jump;
            if (_variableJump)
            {
                _inputHandler.OnJumpRelease -= StopJump;
            }
            _turnCTS?.Cancel();
        }
    }

}
*/