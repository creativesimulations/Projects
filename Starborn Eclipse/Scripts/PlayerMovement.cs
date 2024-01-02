using Cinemachine;
using Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{


public class PlayerMovement : MonoBehaviour
{

    [SerializeField] private float _speed = 5f;
    [SerializeField] private int _speedRate = 2;

        private Rigidbody _rb;
        private Transform _cameraTransform;
        private InputManager _inputManager;
        private CinemachineInputProvider _CMProvider;

        // Start is called before the first frame update
        void Start()
        {
            _rb = GetComponent<Rigidbody>();
            _cameraTransform = Camera.main.transform;
            _CMProvider = FindObjectOfType<CinemachineInputProvider>();
            _CMProvider.enabled = false;
            _inputManager = Singleton.instance.GetComponent<InputManager>();
            _inputManager._inputActions.PlayerInput.SpeedRate.performed += ChangeMovementSpeed;
            _inputManager._inputActions.PlayerInput.AllowMovement.started += AllowMovement;
            _inputManager._inputActions.PlayerInput.AllowMovement.canceled += DisAllowMovement;
        }


        public void ChangeMovementSpeed(InputAction.CallbackContext context)
        {
            _speed *= _speedRate;
        }

        private void Move()
        {
            Vector3 _movementInput = _inputManager.GetPlayerMovement();
            _movementInput = _cameraTransform.forward * _movementInput.z + _cameraTransform.right * _movementInput.x + _cameraTransform.up * _movementInput.y;

            if (_movementInput == Vector3.zero)
            {
                if (_rb.velocity.magnitude > .01f)
                {
                    _rb.velocity -= _rb.velocity / 10f;
                }
            }
            else
            {
                _rb.AddForce(new Vector3(_movementInput.x, _movementInput.y, _movementInput.z) * _speed, ForceMode.Force);
            }
        }
        private void AllowMovement(InputAction.CallbackContext context)
        {
            _CMProvider.enabled = true;
        }
        private void DisAllowMovement(InputAction.CallbackContext context)
        {
            _CMProvider.enabled = false;
        }
        private void FixedUpdate()
        {
            Move();
        }

        private void OnDisable()
        {
            _inputManager._inputActions.PlayerInput.SpeedRate.performed -= ChangeMovementSpeed;
            _inputManager._inputActions.PlayerInput.AllowMovement.started -= AllowMovement;
            _inputManager._inputActions.PlayerInput.AllowMovement.started -= DisAllowMovement;
        }
    }
}
