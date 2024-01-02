using EpicBall;
using TLGFPowerJoysticks;
using UnityEngine;

namespace Core
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(Canvas))]

    public class ControlsManager : MonoBehaviour
    {
        [Header("The joystick components.")]
        [Tooltip("The Power Joystick.")]
        [SerializeField] private PowerJoystick _powerJoystick;
        [Tooltip("The Jump button.")]
        [SerializeField] private PowerButton _jumpButton;

        private Animator _animator;
        private Canvas _canvas;
        private PowerJoystickInputManager _inputManager;

        void Start()
        {
                SetComponents();
#if UNITY_STANDALONE_WIN || UNITY_IOS
            gameObject.SetActive(false);
#endif
#if UNITY_ANDROID || UNITY_IPHONE
            ChangeControlDirection(PlayerPrefsController.GetControlsDirection());
                GameManager.CompleteLvl += FadeControls;
                GameManager.Pause += FadeControls;
                GameManager.PlayGame += UnFadeControls;
                GameManager.Die += FadeControls;
                GameManager.ControlsDirection += ChangeControlDirection;
#endif
        }

        /// <summary>
        /// Sets the required fields.
        /// </summary>
        public void SetComponents()
        {
            _inputManager = GetComponentInChildren<PowerJoystickInputManager>();
            _animator = GetComponent<Animator>();
            _animator.updateMode = AnimatorUpdateMode.UnscaledTime;
            _canvas = GetComponent<Canvas>();
        }

        /// <summary>
        /// Changes the alignment of the controls.
        /// </summary>
        /// <param name="direction"></param>
        public void ChangeControlDirection(int direction)
        {
            if (_powerJoystick == null)
            {
                Singleton.instance.GetComponent<ExceptionManager>().SendMissingObjectMessage("_powerJoystick", GetType().ToString(), name);
                return;
            }
            if (_jumpButton == null)
            {
                Singleton.instance.GetComponent<ExceptionManager>().SendMissingObjectMessage("_jumpButton", GetType().ToString(), name);
                return;
            }
            if (direction == 1)
            {
                _powerJoystick.SetJoystickLandscapeAlignment((PowerJoystick.JoystickLandscapeAlignment)1);
                _jumpButton.SetButtonLandscapeAlignment(0);
            }
            else
            {
                _powerJoystick.SetJoystickLandscapeAlignment(0);
                _jumpButton.SetButtonLandscapeAlignment((PowerButton.ButtonLandscapeAlignment)1);
            }
        }

        /// <summary>
        /// Stops all coroutines and disables the controls.
        /// </summary>
        public void DisableControls()
        {
            if (_inputManager != null)
            {
                _inputManager.StopAllCoroutines();
                _canvas.sortingOrder = -1;
            }
            else
            {
                Singleton.instance.GetComponent<ExceptionManager>().SendMissingComponentMessage("_inputManager", this.GetType().ToString(), name);
            }
        }

        /// <summary>
        /// Enables the controls.
        /// </summary>
        public void EnableControls()
        {
            if (Application.platform != RuntimePlatform.WindowsPlayer || Application.platform != RuntimePlatform.WindowsEditor)
            {
                _canvas.sortingOrder = 1;
            }
        }

        /// <summary>
        /// Activates the animation to pull the controls out of the way.
        /// </summary>
        public void FadeControls()
        {
            _animator.SetBool("FadeController", true);
        }

        /// <summary>
        /// Activates the animation to bring the controls back.
        /// </summary>
        public void UnFadeControls()
        {
                _animator.SetBool("FadeController", false);
        }
#if UNITY_ANDROID || UNITY_IPHONE
        private void OnDisable()
        {
            GameManager.CompleteLvl -= FadeControls;
            GameManager.Pause -= FadeControls;
            GameManager.PlayGame -= UnFadeControls;
            GameManager.Die -= FadeControls;
            GameManager.ControlsDirection -= ChangeControlDirection;
        }
#endif
    }
}