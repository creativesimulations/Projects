using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

namespace EpicBall
{
    public class LevelSceneController : MonoBehaviour
    {
        public static event Action<string> OnChooseLvl;

        [Header("This script is put on a level scene to control the buttons and level choosing.")]
        [Tooltip("A scriptable object with the list of levels to be displayed in this scene.")]
        [SerializeField] private LevelsScriptable _levels;
        [Tooltip("The array of level buttons to be chosen from.")]
        [SerializeField] private Button[] _levelButtons;
        [Tooltip("The gold star image to be used to display on the level buton if achieved.")]
        [SerializeField] private Sprite _bronzeMedal;
        [Tooltip("The silver star image to be used to display on the level buton if achieved.")]
        [SerializeField] private Sprite _silverMedal;
        [Tooltip("The bronze star image to be used to display on the level buton if achieved.")]
        [SerializeField] private Sprite _goldMedal;

        private ExceptionManager _exceptionManager;

        void Start()
        {
            _exceptionManager = Singleton.instance.GetComponent<ExceptionManager>();
            SetupButtons();
        }

        /// <summary>
        /// Sets the correct buttons to be active and displays the proper information on each button.
        /// </summary>
        private void SetupButtons()
        {
            if (_levelButtons.Length == 0)
            {
                _exceptionManager.SendMissingObjectMessage("_levelButtons", this.GetType().ToString(), name);
                return;
            }
            if (_levels == null)
            {
                _exceptionManager.SendMissingObjectMessage("_levels", this.GetType().ToString(), name);
                return;
            }
            // Set the best time, medal image, gold gem ibackground and level name for each button in the array of buttons attached to this script.
            for (int i = 0; i < _levelButtons.Length; i++)
            {
                Button currentButton = _levelButtons[i];
                LevelSettingsScriptable level = _levels._levelSettings[i];
                int bestTime = level._bestTime;

                // If the "active" parameter on the scriptable object for a given level is not set as active, then make its button non-interactable.
                if (!level._isActive)
                {
                    currentButton.interactable = false;
                }

                // Otherwise, activate the button.
                else
                {
                    SetLevelTime(currentButton, bestTime);
                    SetButtonMedal(currentButton, bestTime, level);
                }

                // If the gold gem for this level hasn't been collected, then turn off the special background image.
                if (!level._goldGemCollected)
                {
                    currentButton.transform.Find("Background").GetComponentInParent<RawImage>().enabled = false;
                }
            }

        }

        /// <summary>
        /// Sets the proper medal to be displayed on a given button.
        /// </summary>
        /// <param name="currentButton"></param> The button to be displayed on.
        /// <param name="bestTime"></param> the best time score to be displayed.
        /// <param name="level"></param> The scriptable level settings associated to this specific button.
        private void SetButtonMedal(Button currentButton, int bestTime, LevelSettingsScriptable level)
        {
            // If the level has been played before.
            if (bestTime != 0)
            {
                // Get the medal that was won for this level and diplay it.
                string medal = level._medal;
                GameObject background = currentButton.transform.GetChild(1).gameObject;
                GameObject star = background.transform.GetChild(0).gameObject;
                if (medal == GlobalConstants.GOLD_MEDAL)
                {
                    if (_goldMedal == null)
                    {
                        _exceptionManager.SendMissingObjectMessage("_goldMedal", this.GetType().ToString(), name);
                        return;
                    }
                    star.GetComponent<Image>().sprite = _goldMedal;
                }
                else if (medal == GlobalConstants.SILVER_MEDAL)
                {
                    if (_goldMedal == null)
                    {
                        _exceptionManager.SendMissingObjectMessage("_silverMedal", this.GetType().ToString(), name);
                        return;
                    }
                    star.GetComponent<Image>().sprite = _silverMedal;
                }
                else
                {
                    if (_goldMedal == null)
                    {
                        _exceptionManager.SendMissingObjectMessage("_bronzeMedal", this.GetType().ToString(), name);
                        return;
                    }
                    star.GetComponent<Image>().sprite = _bronzeMedal;
                }
            }
        }

        /// <summary>
        /// Sets the time on the button to the previous best time that the level was completed.
        /// </summary>
        /// <param name="currentButton"></param> The button to be displayed on.
        /// <param name="time"></param> The time score to be displayed.
        private void SetLevelTime(Button currentButton, int time)
        {
            GameObject gO = currentButton.transform.Find(currentButton.name + " Time").gameObject;
            gO.GetComponent<TMPro.TextMeshProUGUI>().text = Timer.StringTime(time);
        }

        /// <summary>
        /// Sends a notification of the level name that was chosen so that it can be loaded.
        /// </summary>
        /// <param name="buttonClicked"></param>
        public void StartLevelOnClick(Button buttonClicked)
        {
            OnChooseLvl?.Invoke(buttonClicked.name);
        }

        /// <summary>
        /// Unloads the easy levels controller scene to go back to the Main Menu on clicking 'Back'.
        /// </summary>
        public void BackOnClick()
        {
            SceneManager.UnloadSceneAsync(gameObject.scene.name);
        }
    }
}