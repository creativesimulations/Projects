using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Furry
{

public class AnimatorStates : MonoBehaviour
    {
        private Animator _animator;
        private string _currentAnimation = "isIdle";
        private PlayerInputHandler _inputHandler;
        private MovementRigidBody _movementRigidBody;
        // Start is called before the first frame update
        void Start()
        {
            _animator = GetComponent<Animator>();
            _inputHandler = GetComponent<PlayerInputHandler>();
            _movementRigidBody = GetComponent<MovementRigidBody>();
            _inputHandler.OnStop += Idle;
            _inputHandler.OnMove += Run;
            _movementRigidBody.OnJump += Jump;
    }

        public void Idle()
        {
            _animator.SetBool(_currentAnimation, false);
            _currentAnimation = "isIdle";
            _animator.SetBool(_currentAnimation, true);
        }
        public void Run()
        {
            _animator.SetBool(_currentAnimation, false);
            _currentAnimation = "isRunning";
            _animator.SetBool(_currentAnimation, true);
        }
        public void Jump()
        {
            _animator.SetTrigger("Jump");
        }
        public void Attack()
        {
            _animator.SetBool(_currentAnimation, false);
            _currentAnimation = "isAttacking";
            _animator.SetBool(_currentAnimation, true);
        }
        public void Die()
        {
            _animator.SetBool(_currentAnimation, false);
            _currentAnimation = "is Dead";
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