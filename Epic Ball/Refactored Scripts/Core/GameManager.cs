using System;
using UnityEngine;

namespace EpicBall
{
    public static class GameManager
    {

        public static event Action Pause;
        public static event Action UnPause;
        public static event Action CompleteLvl;
        public static event Action ChangeLvl;
        public static event Action PlayGame;
        public static event Action MainMenu;
        public static event Action Die;
        public static event Action Win;
        public static event Action<int> ControlsDirection;
        public static event Action<float, float> AudioChanges;

        public static bool _isPaused;


        public enum GameStates
        {
            Pause,
            CompleteLvl,
            ChangeLvl,
            Play,
            MainMenu,
            Dead
        }

        public static GameStates _gameStates { get; private set; }

        /// <summary>
        /// Set the current game state, which will then send a notification on the change. 
        /// </summary>
        /// <param name="state"></param> The state to which the game should be changed.
        public static void SetGameState(GameStates state)
        {
            switch (state)
            {
                case GameStates.Pause:
                    OnPause();
                    break;

                case GameStates.CompleteLvl:
                    OnCompleteLvl();
                    break;

                case GameStates.ChangeLvl:
                    OnChangeLvl();
                    break;

                case GameStates.Play:
                    OnPlay();
                    break;

                case GameStates.MainMenu:
                    OnMainMenu();
                    break;

                case GameStates.Dead:
                    OnDead();
                    break;

                default:
                    break;
            }
        }

        /// <summary>
        /// Pauses the game, sets the game state to "Pause" and sends out a notification to that effect.
        /// </summary>
        public static void OnPause()
        {
            _gameStates = GameStates.Pause;
            Pause?.Invoke();
            PauseGameTime();
        }

        /// <summary>
        /// Sets the game state to "Complete level" and sends out a notification to that effect.
        /// </summary>
        private static void OnCompleteLvl()
        {
            _gameStates = GameStates.CompleteLvl;
            CompleteLvl?.Invoke();
        }

        /// <summary>
        /// Sets the game state to "Change level" and sends out a notification to that effect.
        /// </summary>
        private static void OnChangeLvl()
        {
            _gameStates = GameStates.ChangeLvl;
            ChangeLvl?.Invoke();
        }

        /// <summary>
        /// Sets the game state to "Play" and sends out a notification to that effect.
        /// </summary>
        private static void OnPlay()
        {
            _gameStates = GameStates.Play;
            PlayGame?.Invoke();
            UnPauseGameTime();
        }

        /// <summary>
        /// Sets the game state to "Main Menu" and sends out a notification to that effect.
        /// </summary>
        private static void OnMainMenu()
        {
            _gameStates = GameStates.MainMenu;
            MainMenu?.Invoke();
            UnPauseGameTime();
        }

        /// <summary>
        /// Sets the game state to "Dead" and sends out a notification to that effect.
        /// </summary>
        private static void OnDead()
        {
            _gameStates = GameStates.Dead;
            Die?.Invoke();
        }

        /// <summary>
        /// Pauses game time so that all gameobjects stop where they are.
        /// </summary>
        public static void PauseGameTime()
        {
            _isPaused = true;
            Time.timeScale = 0f;
        }

        /// <summary>
        /// Unpauses game time so that all gameobjects resume movement.
        /// </summary>
        public static void UnPauseGameTime()
        {
            _isPaused = false;
            Time.timeScale = 1f;
        }

        /// <summary>
        /// Sets the direction of the controls.
        /// </summary>
        /// <param name="direction"></param> If set to '1', the controls will be right handed. If set to '0' they will be left handed.
        public static void ControlDirection(int direction)
        {
            ControlsDirection?.Invoke(direction);
        }

        /// <summary>
        /// Sets the music and sound levels.
        /// </summary>
        /// <param name="musicVolume"></param> The music volume level.
        /// <param name="soundVolume"></param> The sound volume level.
        public static void AudioVolume(float musicVolume, float soundVolume)
        {
            AudioChanges?.Invoke(musicVolume, soundVolume);
        }

    }

}