using TMPro;
using UnityEngine;

namespace EpicBall
{
    public class TutorialPopupController : MonoBehaviour
    {

        [Header("This script activates the tutorial canvas and displays the relevant text passed to it from the tutorial popup.")]
        [Tooltip("The canvas to activate.")]
        [SerializeField] private GameObject _popUpCanvas;
        [Tooltip("The text area where the text will be displayed.")]
        [SerializeField] private TextMeshProUGUI _tipText;

        private void Awake()
        {
            // Subscribe to be notified when the player enters a tutorial trigger collider.
            TutorialPopup.OnTutEnter += SetText;
        }

        /// <summary>
        /// Pauses the game, sets the tutorial scene text to the text on the relevant tutorial scriptable object and activates the tutorial scene.
        /// </summary>
        /// <param name="text"></param> The text area on the tutorial scene where the text will be displayed to the player.
        private void SetText(string text)
        {
            GameManager.PauseGameTime();

            if (_popUpCanvas == null)
            {
                Singleton.instance.GetComponent<ExceptionManager>().SendMissingObjectMessage("_popUpCanvas", this.GetType().ToString(), name);
                return;
            }
            _popUpCanvas.SetActive(true);
            _tipText.text = text;
        }

        /// <summary>
        /// Sets the game manager to play and notifies all subscribers to resume game play. This method is called when the 'close' button is clicked on the tutorial scene.
        /// </summary>
        public void ResumeOnClick()
        {
            GameManager.SetGameState(GameManager.GameStates.Play);
            if (_popUpCanvas != null)
            {
                _popUpCanvas.SetActive(false);
            }
        }
        private void OnDisable()
        {
            TutorialPopup.OnTutEnter -= SetText;
        }
    }
}