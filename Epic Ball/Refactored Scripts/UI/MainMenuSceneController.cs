using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace EpicBall
{
    public class MainMenuSceneController : MonoBehaviour
    {
        public static event Action OnActivateSecret;
        public static event Action OnExit;

        [Header("The components on the main menu canvas.")]
        [Tooltip("The secret top invisible button for activating all of the levels.")]
        [SerializeField] private Button _topInvisButton;
        [Tooltip("The secret bottom invisible button for activating all of the levels.")]
        [SerializeField] private Button _bottomInvisButton;
        [Tooltip("The image to activate when a star is active on the easy levels button.")]
        [SerializeField] private Image _easyLevelStar;
        [Tooltip("The image to activate when a star is active on the hard levels button.")]
        [SerializeField] private Image _hardLevelStar;
        [Tooltip("The image to activate when a star is active on the skins button.")]
        [SerializeField] private Image _skinsStar;
        [Tooltip("The text field where the high score will be displayed.")]
        [SerializeField] private TextMeshProUGUI _highScoreText;

        private ExceptionManager _exceptionManager;

        private void Start()
        {
            _exceptionManager = Singleton.instance.GetComponent<ExceptionManager>();
            SetStars();
            SetHighScore();
        }

        /// <summary>
        /// Activates the star images if something new is available.
        /// </summary>
        private void SetStars()
        {
            if (_easyLevelStar == null)
            {
                _exceptionManager.SendMissingObjectMessage("_easyLevelStar", this.GetType().ToString(), name);
                return;
            }
            if (_hardLevelStar == null)
            {
                _exceptionManager.SendMissingObjectMessage("_hardLevelStar", this.GetType().ToString(), name);
                return;
            }
            if (_skinsStar == null)
            {
                _exceptionManager.SendMissingObjectMessage("_skinsStar", this.GetType().ToString(), name);
                return;
            }
            SetStars(_easyLevelStar, GlobalConstants.EASY_LEVELS_STAR);
            SetStars(_hardLevelStar, GlobalConstants.HARD_LEVELS_STAR);
            SetStars(_skinsStar, GlobalConstants.SKINS_STAR);
        }

        private void SetHighScore()
        {
            if (_highScoreText == null)
            {
                _exceptionManager.SendMissingObjectMessage("_highScoreText", this.GetType().ToString(), name);
                return;
            }
            _highScoreText.text = PlayerPrefsController.GetHighScore().ToString();
        }

        /// <summary>
        /// Checks if a star should be displayed.
        /// </summary>
        /// <param name="starToActivate"></param> The star image that would be activated.
        /// <param name="keyName"></param> The key name in PlayerPrefs to be checked.
        public void SetStars(Image starToActivate, string keyName)
        {
            if (PlayerPrefsController.CheckForStar(keyName))
            {
                starToActivate.enabled = true;
            }
            else
            {
                starToActivate.enabled = false;
            }
        }

        /// <summary>
        /// Loads the easy levels menu and removes the possible PlayerPrefs key for a star on the easy levels button.
        /// </summary>
        public void EasyLvlsOnClick()
        {
            PlayerPrefsController.RemoveStar(GlobalConstants.EASY_LEVELS_STAR);
            if (_easyLevelStar != null)
            {
                _easyLevelStar.enabled = false;
            }
            SceneManager.LoadSceneAsync(GlobalConstants.EASY_LEVEL_MENU, LoadSceneMode.Additive);
        }

        /// <summary>
        /// Loads the hard levels menu and removes the possible PlayerPrefs key for a star on the hard levels button.
        /// </summary>
        public void HardLvlsOnClick()
        {
            PlayerPrefsController.RemoveStar(GlobalConstants.HARD_LEVELS_STAR);
            if (_hardLevelStar != null)
            {
                _hardLevelStar.enabled = false;
            }
            SceneManager.LoadSceneAsync(GlobalConstants.HARD_LEVEL_MENU, LoadSceneMode.Additive);
        }

        /// <summary>
        /// Loads the option menu scene and unloads the main menu.
        /// </summary>
        public void OptionsOnClick()
        {
            SceneManager.LoadSceneAsync(GlobalConstants.OPTIONS_MENU, LoadSceneMode.Additive);
        }

        /// <summary>
        /// Exits the game.
        /// </summary>
        public void ExitOnClick()
        {
            if (Application.isEditor)
            {
                PlayerPrefsController.ResetPrefs();
                OnExit?.Invoke();
            }
            Application.Quit();
        }

        /// <summary>
        /// Loads the ball skins scene and removes the possible PlayerPrefs key for a star on the ball skins button.
        /// </summary>
        public void BallSkinsOnClick()
        {
            PlayerPrefsController.RemoveStar(GlobalConstants.SKINS_STAR);
            if (_skinsStar != null)
            {
                _skinsStar.enabled = false;
            }
            SceneManager.LoadSceneAsync(GlobalConstants.BALL_SKINS, LoadSceneMode.Additive);
        }

    }
}