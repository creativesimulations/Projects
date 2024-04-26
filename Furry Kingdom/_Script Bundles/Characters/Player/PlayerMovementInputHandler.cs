/*
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Furry
{

    public class PlayerMovementInputHandler : MonoBehaviour
    {
        public event Action OnJumpHeld;
        public event Action OnJumpRelease;
        public event Action OnRightClick;
        public event Action OnMove;
        public event Action OnRBJump;

        [Header("Input Action References")]
        [Tooltip("Input Action Asset")]
        [SerializeField] private InputActionAsset _playerControls;
        [Tooltip("Action Map Name")]
        [SerializeField] public string ActionMapName { get; set; }

        [Header("Action Names")]
        [SerializeField] private string _ability = "Ability";
        [SerializeField] private string _jump = "Jump";
        [SerializeField] private string _movement = "Movement";
        [SerializeField] private string _rightMouse = "MouseMovement";
        [SerializeField] private string _rbJump = "RigidBodyJump";

        private InputAction _abilityAction;
        private InputAction _jumpAction;
        private InputAction _moveAction;
        private InputAction _rightMouseAction;
        private InputAction _rbJumpAction;

        public bool AbilityTriggered { get; private set; }
        public bool JumpTriggered { get; private set; }
        public bool MoveTriggered { get; private set; }
        public Vector2 MoveInput { get; private set; }
        public bool RightMouseClick { get; private set; }

        private void Awake()
        {
            Init();
        }


        public void Init()
        {
            ActionMapName = "Player1Land";
            _abilityAction = _playerControls.FindActionMap(ActionMapName).FindAction(_ability);
            _jumpAction = _playerControls.FindActionMap(ActionMapName).FindAction(_jump);
            _moveAction = _playerControls.FindActionMap(ActionMapName).FindAction(_movement);
            _rightMouseAction = _playerControls.FindActionMap(ActionMapName).FindAction(_rightMouse);
            _rbJumpAction = _playerControls.FindActionMap(ActionMapName).FindAction(_rbJump);
            RegisterInputActions();
        }

        private void RegisterInputActions()
        {
            _abilityAction.performed += context => AbilityTriggered = true;
            _abilityAction.canceled += context => AbilityTriggered = false;

            _jumpAction.started += JumpHeld;
            _jumpAction.canceled += JumpRelease;

            _rbJumpAction.started += context => OnRBJump?.Invoke();

            _moveAction.performed += Move;
            _moveAction.started += Moving;
            _moveAction.canceled += CancelMoving;

            _rightMouseAction.performed += RightClick;
        }

        public void RbJump(InputAction.CallbackContext context)
        {
        }
        public void RightClick(InputAction.CallbackContext context)
        {
            OnRightClick?.Invoke();
        }
        public void JumpHeld(InputAction.CallbackContext context)
        {
            Debug.Log("On Jump held??");
            JumpTriggered = true;
            OnJumpHeld?.Invoke();
        }
        public void JumpRelease(InputAction.CallbackContext context)
        {
            JumpTriggered = false;
            OnJumpRelease?.Invoke();
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
            _abilityAction.Enable();
            _jumpAction.Enable();
            _moveAction.Enable();
            _rightMouseAction.Enable();
            _rbJumpAction.Enable();
        }
        private void OnDisable()
        {
            _abilityAction.Disable();
            _jumpAction.Disable();
            _moveAction.Disable();
            _rightMouseAction.Disable();
            _rbJumpAction.Disable();
        }
    }

}
*/