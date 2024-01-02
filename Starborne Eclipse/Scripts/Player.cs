using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering.LookDev;
using UnityEngine;
using UnityEngine.InputSystem;
using Core;

namespace Player
{

    public class Player : MonoBehaviour
    {


        private PlayerInput _playerInput;
        private InputManager _inputManager;

        private void Awake()
        {
            _playerInput = GetComponent<PlayerInput>();
            Cursor.lockState = CursorLockMode.Locked;
        }

        void Start()
        {
            _inputManager = Singleton.instance.GetComponent<InputManager>();
            _inputManager.EnablePlayerInputActionMap();
            _inputManager._inputActions.PlayerInput.SwitchActionMap.performed += SwitchActionMap;
        }
        private void SwitchActionMap(InputAction.CallbackContext context)
        {
            switch (context.control.ToString())
            {
                case "Key:/Keyboard/1":
                    _playerInput.SwitchCurrentActionMap("Maestro");
                    _inputManager.EnableMaestroActionMap();
                    break;
                case "Key:/Keyboard/2":
                    _playerInput.SwitchCurrentActionMap("PlayerInput");
                    _inputManager.EnablePlayerInputActionMap();
                    break;
                default:
                    break;
            }
        }

        private void OnDisable()
        {
            _inputManager._inputActions.PlayerInput.SwitchActionMap.performed -= SwitchActionMap;
        }

    }
}
