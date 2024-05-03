using UnityEngine;

namespace Furry
{

    public class AnimatorStates : MonoBehaviour
    {
        private Animator _animator;
        private string _currentAnimation;
        private PlayerInputHandler _inputHandler;
        private MovementRigidBody _movementRigidBody;

        private const string _idle = "isIdle";
        private const string _running = "isRunning";
        private const string _jump = "Jump";
        private const string _attacking = "isAttacking";
        private const string _dead = "isDead";

        void Start()
        {
            _currentAnimation = _idle;

            _animator = GetComponent<Animator>();
            _inputHandler = GetComponent<PlayerInputHandler>();
            _movementRigidBody = GetComponent<MovementRigidBody>();

            _inputHandler.OnStop += Idle;
            _inputHandler.OnMove += Run;
            _movementRigidBody.OnJump += Jump;
        }

        /// <summary>
        /// Activate Idle animation.
        /// </summary>
        public void Idle()
        {
            _animator.SetBool(_currentAnimation, false);
            _currentAnimation = _idle;
            _animator.SetBool(_currentAnimation, true);
        }
        /// <summary>
        /// Activate Run animation.
        /// </summary>
        public void Run()
        {
            _animator.SetBool(_currentAnimation, false);
            _currentAnimation = _running;
            _animator.SetBool(_currentAnimation, true);
        }
        /// <summary>
        /// Trigger Jump animation.
        /// </summary>
        public void Jump()
        {
            _animator.SetTrigger(_jump);
        }
        /// <summary>
        /// Activate Attack animation.
        /// </summary>
        public void Attack()
        {
            _animator.SetBool(_currentAnimation, false);
            _currentAnimation = _attacking;
            _animator.SetBool(_currentAnimation, true);
        }
        /// <summary>
        /// Activate Die animation.
        /// </summary>
        public void Die()
        {
            _animator.SetBool(_currentAnimation, false);
            _currentAnimation = _dead;
            _animator.SetBool(_currentAnimation, true);
        }

        private void OnDisable()
        {
            _inputHandler.OnStop -= Idle;
            _inputHandler.OnMove -= Run;
            _inputHandler.OnRBJump -= Jump;
        }
    }

}