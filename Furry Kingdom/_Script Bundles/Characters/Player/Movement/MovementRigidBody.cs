using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace Furry
{
    [RequireComponent(typeof(PlayerInputHandler))]
    public class MovementRigidBody : MonoBehaviour
    {
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
            if (_rb.velocity.magnitude < _maxVelocity)
            {
                Debug.Log("_rb.velocity.magnitude = " + _rb.velocity.magnitude);
                _rb.AddForce(_moveForce * new Vector3(_inputHandler.MoveInput.x, 0, _inputHandler.MoveInput.y), ForceMode.Force);
            }
        }
        public void Move()
        {
        }

        private void GetTurnInput()
        {
            _targetDirection = new Vector3(_inputHandler.MoveInput.x, 0, _inputHandler.MoveInput.y).normalized;
            _targetRotation = Quaternion.LookRotation(_targetDirection, Vector3.up);

            if ((_targetDirection != _newTargetDirection) && !isTurning)
            {
                Turn();
            }
        }

        private async void Turn()
        {
            _newTargetDirection = _targetDirection;
            isTurning = true;
            while (_inputHandler.MoveTriggered)
            {
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
                _rb.AddForce(Vector3.up * _jumpForce, ForceMode.VelocityChange);
            }
        }
        private bool CanJump()
        {
        RaycastHit _hit;
            _raycastOrigin = transform.position + Vector3.up * 0.1f;
            if (Physics.Raycast(_raycastOrigin, Vector3.down, out _hit, .2f, _terrainLayer))
            {
                Debug.Log("true");
                return true;
            }
            else
            {
                Debug.Log("false");
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