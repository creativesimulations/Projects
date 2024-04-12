using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Furry
{

    public class PlayerInputHandler : MonoBehaviour
    {
        public event Action OnJumpHeld;
        public event Action OnJumpRelease;
        public event Action OnRightClick;

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

        private InputAction _abilityAction;
        private InputAction _jumpAction;
        private InputAction _moveAction;
        private InputAction _rightMouseAction;

        public bool AbilityTriggered { get; private set; }
        public bool JumpTriggered { get; private set; }
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

            RegisterInputActions();
        }

        private void RegisterInputActions()
        {
            _abilityAction.performed += context => AbilityTriggered = true;
            _abilityAction.canceled += context => AbilityTriggered = false;

            _jumpAction.started += JumpHeld;
            _jumpAction.canceled += JumpRelease;

            _moveAction.performed += context => MoveInput = context.ReadValue<Vector2>();
            _moveAction.canceled += context => MoveInput = Vector2.zero;

            _rightMouseAction.performed += RightClick;

        }
        public void RightClick(InputAction.CallbackContext context)
        {
            OnRightClick?.Invoke();
        }
        public void JumpHeld(InputAction.CallbackContext context)
        {
            JumpTriggered = true;
            OnJumpHeld?.Invoke();
        }
        public void JumpRelease(InputAction.CallbackContext context)
        {
            JumpTriggered = false;
            OnJumpRelease?.Invoke();
        }

        private void OnEnable()
        {
            _abilityAction.Enable();
            _jumpAction.Enable();
            _moveAction.Enable();
            _rightMouseAction.Enable();
        }
        private void OnDisable()
        {
            _abilityAction.Disable();
            _jumpAction.Disable();
            _moveAction.Disable();
            _rightMouseAction.Disable();
        }
    }

}