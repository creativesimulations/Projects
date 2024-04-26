using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Furry
{

    public class PlayerInputHandler : MonoBehaviour
    {
        public event Action OnMove;
        public event Action OnRBJump;

        [Header("Input Action References")]
        [Tooltip("Input Action Asset")]
        private InputActionAsset _playerControls;
        [Tooltip("Action Map Name")]
        [SerializeField] public string ActionMapName { get; private set; }

        [Header("Action Names")]
        [SerializeField] private string _movement = "Movement";
        [SerializeField] private string _rbJump = "RigidBodyJump";

        private InputAction _moveAction;
        private InputAction _rbJumpAction;

        public bool JumpTriggered { get; private set; }
        public bool MoveTriggered { get; private set; }
        public Vector2 MoveInput { get; private set; }

        private void Awake()
        {
            _playerControls = GetComponent<PlayerInput>().actions;
            string controls = string.Concat("Player", PlayerManager.Players.Count);
            ActionMapName = controls;
            _playerControls.FindActionMap(ActionMapName);
            _moveAction = _playerControls.FindActionMap(ActionMapName).FindAction(_movement);
            _rbJumpAction = _playerControls.FindActionMap(ActionMapName).FindAction(_rbJump);
            RegisterInputActions();
        }

        private void RegisterInputActions()
        {
            _rbJumpAction.started += context => OnRBJump?.Invoke();

            _moveAction.performed += Move;
            _moveAction.started += Moving;
            _moveAction.canceled += CancelMoving;
        }

        public void Moving(InputAction.CallbackContext context)
        {
            MoveTriggered = true;
        }
        public void CancelMoving(InputAction.CallbackContext context)
        {
            MoveInput = Vector2.zero;
            MoveTriggered = false;
        }
        public void Move(InputAction.CallbackContext context)
        {
            MoveInput = context.ReadValue<Vector2>();
            OnMove?.Invoke();
        }
        private void OnEnable()
        {
            _moveAction.Enable();
            _rbJumpAction.Enable();
        }
        private void OnDisable()
        {
            _moveAction.Disable();
            _rbJumpAction.Disable();
        }
    }

}