using System;
using UnityEngine;

namespace Furry
{

    public class GameManager : MonoBehaviour
    {
        public static event Action OnUI;
        public static event Action OnPlay;
        public static event Action OnCompleteLvl;
        public static event Action OnPause;

        public static GameManager Instance;
        public static bool IsPaused;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this);
            }
            else
            {
                Instance = this;
            }
        }
        public enum GameStates
        {
            UI,
            Play,
            CompleteLvl,
            Paused
        }
        public static GameStates GameState { get; private set; }

        /// <summary>
        /// Sets the desired state.
        /// </summary>
        /// <param name="GameState"></param> Desired state.
        public void SetGameState(GameStates GameState)
        {
            SwitchState(GameState);
        }

        /// <summary>
        /// Switches the state to the desired state and calles the related method.
        /// </summary>
        /// <param name="GameState"></param> Desired state.
        private void SwitchState(GameStates GameState)
        {
            switch (GameState)
            {
                case GameStates.UI:
                    UI();
                    break;
                case GameStates.Play:
                    Play();
                    break;
                case GameStates.CompleteLvl:
                    CompleteLvl();
                    break;
                case GameStates.Paused:
                    Paused();
                    break;
            }
        }


        private void Start()
        {
            ProceduralLevelGenerator.OnLevelGenerated += Init;
        }

        /// <summary>
        /// Pauses the game, sets the game state to "Pause" and sends out a notification to that effect.
        /// </summary>
        private void UI()
        {
            // NEED TO TURN OFF THE UI CANVAS  ***
            GameState = GameStates.Paused;
            OnPause?.Invoke();
            //   PauseGameTime();  NOT YET IMPLEMENTED ***
        }

        /// <summary>
        /// Set game state to Play.
        /// </summary>
        private void Play()
        {

        }

        /// <summary>
        /// Sets game state to level is completed.
        /// </summary>
        private void CompleteLvl()
        {
            // NOT YET IMPLEMENTED * **
        }

        /// <summary>
        /// Pauses the game
        /// </summary>
        private void Paused()
        {
            GameState = GameStates.Paused;
            OnPause?.Invoke();
            //   PauseGameTime();  NOT YET IMPLEMENTED ***
        }

        /// <summary>
        /// Run any initialization before setting game state to Play.
        /// </summary>
        private void Init()
        {
            Play();
        }
    }
}
