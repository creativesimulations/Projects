using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

namespace EpicBall
{

    [RequireComponent(typeof(Ball))]

    public class BallUserControl : MonoBehaviour
    {
        private Ball _ball;
        private Vector3 _move;
        private bool _jump;
        private bool _canJump;
        private float _hInput = 0f;
        private float _vInput = 0f;
        private float _currentSpeed = 0;

        private float _vSpeed;
        private float _hSpeed;

        private void Awake()
        {
            _ball = GetComponent<Ball>();
        }

        private void Update()
        {
#if UNITY_ANDROID || UNITY_IPHONE
            if (GameManager._gameStates == GameManager.GameStates.Play && !GameManager._isPaused)
            {
                _hInput = CrossPlatformInputManager.GetAxis("Horizontal");
                _vInput = CrossPlatformInputManager.GetAxis("Vertical");
                _jump = CrossPlatformInputManager.GetButtonDown("Jump");
                if (CrossPlatformInputManager.GetAxisRaw("Horizontal") == 0)
                {
                    _ball.SlowBallx();
                }
                if (CrossPlatformInputManager.GetAxisRaw("Vertical") == 0)
                {
                    _ball.SlowBallz();
                }
                if (CrossPlatformInputManager.GetButtonDown("Pause"))
                {
                    GameManager.SetGameState(GameManager.GameStates.Pause);
                }
                _vSpeed = Math.Abs(_vInput);
                _hSpeed = Math.Abs(_hInput);
                _currentSpeed = Math.Max(_vSpeed, _hSpeed);
                _move = ((_vInput * Vector3.forward) + (_hInput * Vector3.right)).normalized;
            }
#endif
#if UNITY_STANDALONE_WIN || UNITY_IOS
            if (Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor)
            {
                if (GameManager._gameStates == GameManager.GameStates.Play && !GameManager._isPaused)
                {
                    _hInput = Input.GetAxis("Horizontal");
                    _vInput = Input.GetAxis("Vertical");
                    _jump = Input.GetButtonDown("Jump");
                    if (Input.GetAxisRaw("Horizontal") == 0)
                    {
                        _ball.SlowBallx();
                    }
                    if (Input.GetAxisRaw("Vertical") == 0)
                    {
                        _ball.SlowBallz();
                    }
                    if (Input.GetButtonDown("Pause"))
                    {
                        GameManager.SetGameState(GameManager.GameStates.Pause);
                    }
                    _vSpeed = Math.Abs(_vInput);
                    _hSpeed = Math.Abs(_hInput);
                    _currentSpeed = Math.Max(_vSpeed, _hSpeed);
                    _move = ((_vInput * Vector3.forward) + (_hInput * Vector3.right)).normalized;

                }
            }
#endif

            if (GameManager._gameStates == GameManager.GameStates.Play && !GameManager._isPaused)
            {
                if (_jump)
                {
                    if (CheckCanJump())
                    {
                        _ball.BallJump();
                    }
                }
            }
        }

        /// <summary>
        /// If the game state is 'Play, the player can be moved.
        /// </summary>
        private void FixedUpdate()
        {
            if (GameManager._gameStates == GameManager.GameStates.Play)
            {
                _ball.Move(_move, _currentSpeed);
            }
        }

        /// <summary>
        /// Returns whether the player is able to jump.
        /// </summary>
        /// <returns></returns>
        private bool CheckCanJump()
        {
            if (Physics.Raycast(transform.position, -Vector3.up, 0.6f))
            {
                _canJump = true;
            }
            else
            {
                _canJump = false;
            }
            return _canJump;
        }
    }
}