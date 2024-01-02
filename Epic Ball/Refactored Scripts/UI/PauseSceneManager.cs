using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static EpicBall.GameManager;

namespace EpicBall
{
    public class PauseSceneManager : MonoBehaviour
    {
        public static event Action OnRestartLvl;

        [SerializeField] private TextMeshProUGUI _timeDisplay;
        [SerializeField] private Image _skinStar;

        private void Start()
        {
            ShowTime();
            SetStar();
        }

        /// <summary>
        /// Displays the time that has passed so far while playing the current level.
        /// </summary>
        private void ShowTime()
        {
            if (_timeDisplay == null)
            {
                Singleton.instance.GetComponent<ExceptionManager>().SendMissingObjectMessage("_timeDisplay", this.GetType().ToString(), name);
                return;
            }
            _timeDisplay.text = "Paused / Time: " + Timer.StringTime(Timer.GetIntTime());
        }

        /// <summary>
        /// Sets the star image on the skins button if a new skin is available and it hasn't been seen yet by the player.
        /// </summary>
        private void SetStar()
        {
            if (_skinStar == null)
            {
                Singleton.instance.GetComponent<ExceptionManager>().SendMissingObjectMessage("_skinStar", this.GetType().ToString(), name);
                return;
            }
            if (PlayerPrefsController.CheckForStar(GlobalConstants.SKINS_STAR))
            {
                _skinStar.enabled = true;
            }
            else
            {
                _skinStar.enabled = false;
            }
        }

        /// <summary>
        /// Changes the Game Manager state to 'Play' and also unloads the pause additive scene.
        /// </summary>
        public void ResumeOnClick()
        {
            SetGameState(GameStates.Play);
            SceneManager.UnloadSceneAsync(GlobalConstants.PAUSE_MENU);
        }

        /// <summary>
        /// Additively loads the options scene when the 'Options' button is clicked.
        /// </summary>
        public void OptionsOnClick()
        {
            SceneManager.LoadSceneAsync(GlobalConstants.OPTIONS_MENU, LoadSceneMode.Additive);
        }

        /// <summary>
        /// Restarts the current level when the 'Restart' button is clicked.
        /// </summary>
        public void RestartOnClick()
        {
            OnRestartLvl?.Invoke();
        }

        /// <summary>
        /// Changes the Game Manager state to 'MainMenu' when the 'Main Menu' button is clicked.
        /// </summary>
        public void MainMenuOnClick()
        {
            SetGameState(GameStates.MainMenu);
        }

        /// <summary>
        /// Additively loads the ball skins scene when the 'Change Ball' button is clicked.
        /// </summary>
        public void BallSkinsOnClick()
        {
            PlayerPrefsController.RemoveStar(GlobalConstants.SKINS_STAR);
            if (_skinStar != null)
            {
                _skinStar.enabled = false;
            }
            SceneManager.LoadSceneAsync(GlobalConstants.BALL_SKINS, LoadSceneMode.Additive);
        }

    }
}