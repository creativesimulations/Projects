using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace EpicBall
{
    public class BallSkinController : MonoBehaviour
    {

        public static event Action ChoseSkin;

        [Tooltip("An array of the skin buttons in the scene")]
        [SerializeField] private Button[] _skinButtons;

        private void Start()
        {
            SetButtons();
        }

        /// <summary>
        /// Unloads the skins option scene. This method is run when clicking 'back' on the skins scene.
        /// </summary>
        public void BackOnClick()
        {
            SceneManager.UnloadSceneAsync(GlobalConstants.BALL_SKINS);
        }

        /// <summary>
        /// Changes the active skin on the player. This method is run when a skin button is pressed.
        /// </summary>
        /// <param name="button"></param> The button that was clicked
        public void ChangeSkinOnClick(Button button)
        {
            int buttonNumber = Array.IndexOf(_skinButtons, button);
            if (_skinButtons[buttonNumber].interactable)
            {
                PlayerPrefsController.SetChosenSkin(buttonNumber);

                ChoseSkin?.Invoke();
                BackOnClick();
            }
        }

        /// <summary>
        /// Sets up the buttons to be interactable or not.
        /// </summary>
        private void SetButtons()
        {
            if (_skinButtons.Length == 0)
            {
                Singleton.instance.GetComponent<ExceptionManager>().SendEmptyContainerMessage("_skinButtons", this.GetType().ToString(), name);
                return;
            }
            for (int i = 0; i < _skinButtons.Length; i++)
            {
                if (i > PlayerPrefsController.GetSkinAmountAvailable())
                {
                    _skinButtons[i].interactable = false;
                }
                else
                {
                    GameObject skinButton = _skinButtons[i].gameObject;
                    skinButton.GetComponentInChildren<TMPro.TextMeshProUGUI>().enabled = false;
                }
            }
        }
    }
}