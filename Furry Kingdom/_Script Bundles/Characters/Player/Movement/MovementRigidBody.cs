using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace Furry
{
    [RequireComponent(typeof(PlayerInputHandler))]
    public class MovementRigidBody : MonoBehaviour
    {
        [SerializeField, Range(1, 20)] private float _moveSpeed = 5;
        [SerializeField, Range(1, 20)] private float _jumpHeight = 6.5f;
        [SerializeField] private bool _variableJump;
        [SerializeField] private float _rotationSpeed = 5f; // Speed of rotation

        private bool _isJumping;
        private bool isTurning;
        private float _velocity;
        private float _gravityScale = 5;

        private Vector3 _targetDirection; // The direction the player is moving towards
        private Vector3 _newTargetDirection; // The direction the player is moving towards
        private Quaternion _targetRotation;

        private Vector3 _raycastOrigin;
        private RaycastHit _hit;
        [SerializeField] private LayerMask _terrainLayer;

        private PlayerInputHandler _inputHandler;
        private CancellationTokenSource _turnCTS;
        private Rigidbody _rb;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
            _turnCTS = new CancellationTokenSource();
            _inputHandler = GetComponent<PlayerInputHandler>();
        }

        private void Start()
        {
            _inputHandler.OnMove += GetTurnInput;
            _inputHandler.OnRBJump += RBJump;
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
        private void RBJump()
        {
            if (CanJump())
            {
                _rb.AddForce(Vector3.up * _jumpHeight, ForceMode.Impulse);
            }
        }
        private bool CanJump()
        {
            _raycastOrigin = transform.position + Vector3.up * 0.1f;
            if (Physics.Raycast(_raycastOrigin, Vector3.down, out _hit, .2f, _terrainLayer))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void OnDisable()
        {
            _inputHandler.OnMove -= GetTurnInput;
            _inputHandler.OnRBJump -= RBJump;
            _turnCTS?.Cancel();
        }
    }

}