using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace EpicBall
{
    public class OptionsSceneController : MonoBehaviour
    {

        [Header("The canvas objects for the options scene.")]
        [Tooltip("The left toggle image container for the button.")]
        [SerializeField] private Image _leftToggleImage;
        [Tooltip("The right toggle image container for the button.")]
        [SerializeField] private Image _rightToggleImage;
        [Tooltip("The active image for the button.")]
        [SerializeField] private Sprite _activeImage;
        [Tooltip("The deactivae image for the button.")]
        [SerializeField] private Sprite _deactiveImage;

        [SerializeField] private Slider _musicSlider;
        [SerializeField] private Slider _soundSlider;

        private void Awake()
        {
            SetSliders();
        }

        private void Start()
        {
            SetUpButtons();
        }

        /// <summary>
        /// Set the controls direction from the saved PlayerPrefs.
        /// </summary>
        public void SetUpButtons()
        {
            if (_rightToggleImage == null)
            {
                Singleton.instance.GetComponent<ExceptionManager>().SendMissingObjectMessage("_rightToggleImage", this.GetType().ToString(), name);
                return;
            }
            if (_leftToggleImage == null)
            {
                Singleton.instance.GetComponent<ExceptionManager>().SendMissingObjectMessage("_leftToggleImage", this.GetType().ToString(), name);
                return;
            }
            if (_activeImage == null)
            {
                Singleton.instance.GetComponent<ExceptionManager>().SendMissingObjectMessage("_activeImage", this.GetType().ToString(), name);
                return;
            }
            if (_deactiveImage == null)
            {
                Singleton.instance.GetComponent<ExceptionManager>().SendMissingObjectMessage("_deactiveImage", this.GetType().ToString(), name);
                return;
            }
            if (PlayerPrefsController.GetControlsDirection() == 1)
            {
                _rightToggleImage.sprite = _activeImage;
                _leftToggleImage.sprite = _deactiveImage;
            }
            else
            {
                _leftToggleImage.sprite = _activeImage;
                _rightToggleImage.sprite = _deactiveImage;
            }
        }

        /// <summary>
        /// Loads the main menu scene and unloads the options menu scene when 'Back' is clicked.
        /// </summary>
        public void BackOnClick()
        {
            SceneManager.UnloadSceneAsync(GlobalConstants.OPTIONS_MENU);
        }

        /// <summary>
        /// Loads the Tips scene when 'Tips' is clicked.
        /// </summary>
        public void TipsClick()
        {
            SceneManager.LoadSceneAsync(GlobalConstants.TIPS_MENU, LoadSceneMode.Additive);
        }

        /// <summary>
        /// Changes the music volume when the music volume slider is moved.
        /// </summary>
        /// <param name="musicVolume"></param> The value of the music volume slider.
        public void ChangeMusicVolume(float musicVolume)
        {
            GameManager.AudioVolume(musicVolume, PlayerPrefsController.GetSoundVolume());
        }

        /// <summary>
        /// Changes the sound volume when the sound volume slider is moved.
        /// </summary>
        /// <param name="soundVolume"></param> The value of the sound volume slider.
        public void ChangeSoundVolume(float soundVolume)
        {
            GameManager.AudioVolume(PlayerPrefsController.GetMusicVolume(), soundVolume);
        }

        /// <summary>
        /// Sets the slider values as per the saved information in PlayerPrefs.
        /// </summary>
        private void SetSliders()
        {
            if (_musicSlider != null)
            {
                _musicSlider.value = PlayerPrefsController.GetMusicVolume();
            }
            else
            {
                Singleton.instance.GetComponent<ExceptionManager>().SendMissingObjectMessage("_musicSlider", this.GetType().ToString(), name);
            }
            if (_soundSlider != null)
            {
                _soundSlider.value = PlayerPrefsController.GetSoundVolume();
            }
            else
            {
                Singleton.instance.GetComponent<ExceptionManager>().SendMissingObjectMessage("_soundSlider", this.GetType().ToString(), name);
            }
        }

        /// <summary>
        /// Activates the right toggle button and sets the controls to right handed. This method is run from an event on the right button.
        /// </summary>
        public void RightControls()
        {
            PlayerPrefsController.SetControlsDirection(1);
            GameManager.ControlDirection(1);
            _rightToggleImage.sprite = _activeImage;
            _leftToggleImage.sprite = _deactiveImage;
        }

        /// <summary>
        /// Activates the left toggle button and sets the controls to left handed. This method is run from an event on the left button.
        /// </summary>
        public void LeftControls()
        {
            PlayerPrefsController.SetControlsDirection(0);
            GameManager.ControlDirection(0);
            _leftToggleImage.sprite = _activeImage;
            _rightToggleImage.sprite = _deactiveImage;
        }
    }
}