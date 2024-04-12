using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Furry
{
    [RequireComponent(typeof(PlayerInputHandler))]
        public class MovementTransport : MonoBehaviour
    {
        [SerializeField, Range(1, 20)] private float _moveSpeed = 5;
        [SerializeField, Range(1, 20)] private float _jumpHeight = 6.5f;
        [SerializeField] private bool _variableJump;

        private bool _isJumping;
        private float _velocity;
        private float _gravityScale = 5;
        private RaycastHit hit;

        private PlayerInputHandler _inputHandler;
        private void Awake()
        {
            _inputHandler = GetComponent<PlayerInputHandler>();
        }

        private void Start()
        {
            _inputHandler.OnJumpHeld += Jump;
            if (_variableJump)
            {
                _inputHandler.OnJumpRelease += StopJump;
            }
        }

        private void FixedUpdate()
        {
            Move();
        }

        public void Move()
        {
            transform.position += _moveSpeed * Time.deltaTime * new Vector3(_inputHandler.MoveInput.x, 0, _inputHandler.MoveInput.y);
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
        }
    }

}