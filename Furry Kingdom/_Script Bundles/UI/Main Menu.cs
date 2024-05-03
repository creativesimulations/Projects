using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Furry
{
    public class MainMenu : MonoBehaviour
    {
        /// <summary>
        /// Notice is sent with the desired player amount when the player clicks one of the buttons that chooses how many players they want in the game.
        /// </summary>
        public static event Action<int> OnChoosePlayerNum;

        private Canvas _canvas;
        private void Awake()
        {
            _canvas = GetComponentInChildren<Canvas>();
        }

        /// <summary>
        /// Disables the UI canvas, Loads the game level and invokes the action. The method is run when one of the UI buttons is pressed.
        /// </summary>
        /// <param name="numPlayers"></param> The amount of players desired.
        public void ChoosePlayer(int numPlayers)
        {
            OnChoosePlayerNum?.Invoke(numPlayers);
            DisableCanvas();
            LoadLevel();
        }

        /// <summary>
        /// Disables the UI canvas.
        /// </summary>
        private void DisableCanvas()
        {
            _canvas.gameObject.SetActive(false);
        }

        /// <summary>
        /// Enables the UI canvas
        /// </summary>
        private void EnableCanvas()
        {
            _canvas.gameObject.SetActive(true);
        }

        /// <summary>
        /// Loads the game level additively.
        /// </summary>
        private void LoadLevel()
        {
            SceneManager.LoadSceneAsync("GameLevel", LoadSceneMode.Additive);
        }
    }

}